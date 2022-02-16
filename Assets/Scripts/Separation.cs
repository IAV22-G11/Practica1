using System.Collections.Generic;
using UnityEngine;

namespace UCM.IAV.Movimiento
{
    /// <summary>
    /// Clase para modelar el comportamiento de Llegar a otro agente
    /// </summary>
    public class Separation : ComportamientoAgente
    {

        List<Transform> tRatas = new List<Transform>();

        [SerializeField]
        float threshold;

        [SerializeField]
        float decayCoefficient;

        private Vector3 direction;
        private float distance;
        private float strength;

        private void Start()
        {
            //Cogemos el padre que gestiona las ratas
            Transform pRatas = this.transform.parent;

            //Y vemos el numero de hijos que tiene
            int numRatas = pRatas.GetChildCount();

            //Añadimos a la lista el transform de las otras ratas
            for (int i = 0; i < numRatas; i++)
            {
                //Con este if evitamos que se añada a si misma
                if (pRatas.GetChild(i) != this.transform)
                    tRatas.Add(pRatas.GetChild(i));
            }
        }

        /// <summary>
        /// Obtiene la dirección
        /// </summary>
        /// <returns></returns>
        public override Direccion GetDireccion()
        {

            Direccion result = new Direccion();

            for (int i = 0; i < tRatas.Count; i++)
            {
                direction = this.transform.position - tRatas[i].position;
                distance = direction.magnitude;

                if (distance < threshold)
                {
                    //Calculate the strength of repulsion
                    //here using the inverse square law
                    strength = Mathf.Min(
                    decayCoefficient / (distance * distance),
                    agente.aceleracionMax);

                    //Add the acceleration.
                    direction.Normalize();
                    result.lineal += strength * direction;
                }
            }

            result.lineal.y = 0;

            return result;

        }
    }
}
