using UnityEngine;

namespace UCM.IAV.Movimiento
{

    /// <summary>
    /// Clase para modelar el comportamiento de Merodear
    /// </summary>
    public class Merodear : ComportamientoAgente
    {
        /// <summary>
        /// Obtiene la dirección
        /// </summary>
        /// <returns></returns>
        public override Direccion GetDireccion()
        {
            Direccion result = new Direccion();

            //Sacamos la velocidad a partir de la forma vectorial de la orientacion
            result.lineal = agente.velocidadMax * agente.OriToVec(agente.orientacion);

            //Cambiamos nuestra orientacion aleatoriamente
            int numAleatorio;
            if (Random.value < 0.5)
            {
                numAleatorio = -1;
            } else
                numAleatorio = 1;
            result.angular = numAleatorio * agente.rotacionMax;

            return result;
        }
    }
}
