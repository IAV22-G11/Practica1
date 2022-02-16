using UnityEngine;

namespace UCM.IAV.Movimiento
{

    /// <summary>
    /// Clase para modelar el comportamiento de Llegar a otro agente
    /// </summary>
    public class Llegar : ComportamientoAgente
    {
        float radius = 2;
        float timeToTarget = 0.25f;

        //Para que las ratas formen
        public float threshold;
        private float thresholdAux; //Se usa para evitar que haga calculos de la misma rata
        public float decayCoefficient;

        private float strength;
        private float distance;

        public GameObject padreRatas;
        Transform[] tRatas;

        private void Start()
        {
            //Cogemos la posicion de todas las ratas
            tRatas = padreRatas.GetComponentsInChildren<Transform>();
        }



        /// <summary>
        /// Obtiene la dirección
        /// </summary>
        /// <returns></returns>
        public override Direccion GetDireccion()
        {
            Direccion result = new Direccion();
            result.lineal = objetivo.transform.position - transform.position;

            //Si ya hemos llegado se para de mover
            if (result.lineal.magnitude < radius)
            {
                result.lineal = new Vector3();
            }
            else
            {
                //Para llegar al objetivo en timeTotarget segundos
                result.lineal /= timeToTarget;

                //Comprobamos que no pase de la velocidad máxima
                if (result.lineal.magnitude > agente.velocidadMax)
                {
                    result.lineal.Normalize();
                    result.lineal *= agente.velocidadMax;
                }
            }


            ////Si son las ratas...
            //if (this.gameObject.GetComponent<Merodear>() != null)
            //{
            //    foreach (Transform trans in tRatas)
            //    {
            //        distance = (trans.position - transform.position).magnitude;
            //        if (distance < threshold && distance > thresholdAux)
            //        {
            //            //Calculate the strength of repulsion
            //            //here using the inverse square law
            //            strength = Mathf.Min(decayCoefficient / (distance * distance), agente.aceleracionMax);

            //            //Add the acceleration.
            //            result.lineal.Normalize();
            //            result.lineal += strength * result.lineal;
            //        }
            //    }
            //}

            return result;

        }
    }
}
