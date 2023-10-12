using UnityEngine;

namespace Assets.Scripts
{
    public class DelegateExample : MonoBehaviour
    {
        ///accessiblity  delegate returntype method(parameters)
        public delegate float DoMath(float a, float b);

        public void Awake()
        {
            DoMath domathvariable = new DoMath(Add);

            Debug.Log($"ADD METHOD : {domathvariable(14, 15)} ");
            //Debug.Log("ADD METHOD : " + domathvariable(14, 15));

            domathvariable = Subtract;

            Debug.Log("Subtract: " + domathvariable(14, 15));

            domathvariable = (a, b) => a * b;

            Debug.Log("Multiply: " + domathvariable(14, 15));
        }

        public float ADDandSubtract(DoMath adding, DoMath Subtracting)
        {
            return adding(14, 15) + Subtracting(14, 15);
        }

        public float Multiply (float a, float b)
        {
            return a * b;
        }

        public float Add(float x, float y)
        {
            return x + y;
        }

        public float Subtract(float a, float b)
        {
            return a - b;
        }

    }
}
