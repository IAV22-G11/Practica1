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
            return result;

        }
    }
}
