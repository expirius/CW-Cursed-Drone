using UnityEngine;
using UnityEngine.Events;

namespace CW_Cursed_Drone
{
    public enum DronePhysicsFlightConfiguration
    {
        AltitudeHold
    };
    [RequireComponent(typeof(Rigidbody))]
    public class DronePhysicsBehaviour : MonoBehaviour
    {
        public DronePhysicsFlightConfiguration flightConfig = 0;

        public bool armed = true;
        private bool lastArmed = true;

        public DronePhysicsConfiguration physicsConfig = new();
        public LayerMask hoverEffectLayerMask;

        public UnityEvent OnArmed;
        public UnityEvent OnDisarmed;

        private Transform targetQuad;

        private DronePIDController rotationPID;
        private DronePIDController altitudePID;

        private float targetAltitude;

        [HideInInspector]
        public Vector3 appliedTorque;
        [HideInInspector]
        public Vector3 appliedForce;

        private Rigidbody rb;

        void Start()
        {
            // Get References
            rb = GetComponent<Rigidbody>();

            // Instantiate Target Transform
            targetQuad = new GameObject("TargetQuad").transform;
            targetQuad.parent = transform;
            targetQuad.SetPositionAndRotation(transform.position, transform.rotation * Quaternion.Euler(0, 90, 0));

            rotationPID = new DronePIDController();
            altitudePID = new DronePIDController();
        }
        private void FixedUpdate()
        {
            if (armed)
            {
                // Process Input
                switch (flightConfig)
                {
                    case DronePhysicsFlightConfiguration.AltitudeHold:
                        AltitudeHold();
                        break;
                }

                // Process Error, PID
                appliedTorque = CalculatePIDTorque();

                // Apply Torque
                if (appliedTorque.magnitude > 0f)
                    rb.AddRelativeTorque(appliedTorque);

                // Apply Force
                if (appliedForce.magnitude > physicsConfig.thrust)
                    appliedForce = appliedForce.normalized * physicsConfig.thrust;

                rb.AddForce(appliedForce.y * transform.up);

                // Update HoverEffect
                UpdateHoverEffect();

                // Update Rigigbody Configuration
                UpdateRigidbody();

                // Check EventTrigger
                if (lastArmed != armed)
                {
                    if (armed)
                    {
                        OnQuadArmed();
                    }
                    else
                    {
                        OnQuadDisarmed();
                    }

                    lastArmed = armed;
                }

                // Check for Errors
                CheckErrors();
            }
        }
        public void OnPhysicsConfigurationChanged(DronePhysicsConfiguration config)
        {
            physicsConfig = config;
        }
        private void UpdateRigidbody()
        {
            rb.mass = physicsConfig.mass;
            rb.drag = physicsConfig.drag;
            rb.angularDrag = physicsConfig.angularDrag;
        }

        private void CheckErrors()
        {
            if (rb.mass < 0.01f)
                Debug.Log($"[Cursed Drone {nameof(DronePhysicsBehaviour)}] Drone Mass is unrealistically low. Change in PhysicsConfiguration.");
        }

        private void AltitudeHold()
        {
            targetAltitude = transform.position.y; // Автоматическое поддержание текущей высоты

            float error = targetAltitude - transform.position.y; // Рассчет ошибки положения

            float pdForce;

            if (Mathf.Abs(error) > 0.5f)
            {
                targetAltitude = transform.position.y + 0.5f;
            }

            pdForce = altitudePID.CalculatePD(error * Vector3.one, physicsConfig.pAltitude, physicsConfig.dAltitude).y;

            if (pdForce < 0)
                pdForce = 0;

            appliedForce = pdForce * Vector3.up; // Применение силы

            // Стабилизация углового положения (возврат в нейтральное положение)
            targetQuad.localRotation = Quaternion.RotateTowards
            (
                targetQuad.localRotation,
                Quaternion.identity,
                physicsConfig.autoStabilizeSpeed * Time.deltaTime
            );
        }

        private void UpdateHoverEffect()
        {
            if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, 0.3f, hoverEffectLayerMask))
            {
                float hoverFactor = (-(hit.distance / 0.3f) + 1) * 0.25f;
                rb.AddForce(appliedForce.y * transform.up * hoverFactor);
            }
        }

        private Vector3 CalculatePIDTorque()
        {
            Vector3 torque;

            // Calculate Magnitude and Axis
            Quaternion delta = targetQuad.localRotation * Quaternion.Inverse(transform.rotation);

            delta.ToAngleAxis(out float errorMagnitude, out Vector3 axis);

            // Clip errorMagnitude 
            if (errorMagnitude > 180)
                errorMagnitude -= 360;

            if (errorMagnitude < -180)
                errorMagnitude += 360;

            // Calculate Error
            Vector3 error = axis * errorMagnitude;

            // Calculate PID
            torque = rotationPID.CalculatePD(error, physicsConfig.p, physicsConfig.d);
            torque = transform.InverseTransformDirection(torque);

            return torque;
        }

        private void OnQuadArmed()
        {
            OnArmed.Invoke();
            ResetInternals();
        }

        private void OnQuadDisarmed()
        {
            OnDisarmed.Invoke();
            ResetInternals();
        }

        public void ResetInternals()
        {
            targetAltitude = transform.position.y;
            targetQuad.transform.rotation = transform.rotation * Quaternion.Euler(0, 90, 0);
        }
    }
}
