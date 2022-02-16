/*    
   Copyright (C) 2020-2021 Federico Peinado
   http://www.federicopeinado.com

   Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
   Esta asignatura se imparte en la Facultad de Informática de la Universidad Complutense de Madrid (España).

   Autor: Federico Peinado 
   Contacto: email@federicopeinado.com
*/
using UnityEngine;

namespace UCM.IAV.Movimiento
{

    /// <summary>
    /// Clase para modelar el comportamiento de HUIR a otro agente
    /// </summary>
    public class Huir : ComportamientoAgente
    {
        Llegar llegarScript;

        int numRatas = 0;
        int numRatasProcesado = 0;
        UnityEngine.Vector3 centroRatas;

        [UnityEngine.SerializeField]
        int maxRatas = 3;

        //Con esto hacemos que cuando empiece a huir se vaya unos segundos para que no se esté chocando continuamente con el trigger
        float timer = 0;
        
        private void Start()
        {
            llegarScript = GetComponent<Llegar>();
            centroRatas = UnityEngine.Vector3.zero;
        }

        private void Update()
        {
            if (timer > 0)
            {
                timer -= Time.time;
            }
            else
            {
                llegarScript.cambiaPerseguir(true);
            }
        }

        /// <summary>
        /// Obtiene la dirección
        /// </summary>
        /// <returns></returns>
        public override Direccion GetDireccion()
        {
            // Si fuese un comportamiento de dirección dinámico en el que buscásemos alcanzar cierta velocidad en el agente, se tendría en cuenta la velocidad actual del agente y se aplicaría sólo la aceleración necesaria
            // Vector3 deltaV = targetVelocity - body.velocity;
            // Vector3 accel = deltaV / Time.deltaTime;
            Direccion direccion = new Direccion();
            if (numRatasProcesado != 0)
            {
                direccion.lineal = transform.position - centroRatas / numRatasProcesado;
                //Hay objetos con alturas diferences, haciendo esto evitamos que levite o se hunda
                direccion.lineal.y = 0;
                centroRatas = UnityEngine.Vector3.zero;
            }
            else
                direccion.lineal = UnityEngine.Vector3.zero;
            direccion.lineal.Normalize();
            direccion.lineal *= agente.aceleracionMax;

            numRatasProcesado = 0;

            // Podríamos meter una rotación automática en la dirección del movimiento, si quisiéramos
            return direccion;
        }
        private void OnTriggerEnter(UnityEngine.Collider other)
        {
            //Si entra una rata nueva en el trigger...
            if (other.GetComponent<Merodear>() != null)
            {
                //Se suma el numero de ratas
                numRatas++;

                //Si hay demasiadas ratas...
                if (numRatas >= maxRatas)
                {
                    //Empieza a huir
                    llegarScript.prioridad = 2;
                    timer = 1200;
                    llegarScript.cambiaPerseguir(false);
                }
            }
        }

        private void OnTriggerStay(UnityEngine.Collider other)
        {
            //Si ya hemos procesado a todas...
            if (numRatasProcesado < numRatas)
            {
                //Si las ratas siguen ahí dentro...
                if (other.GetComponent<Merodear>() != null)
                {
                    //Se guarda la posicion de la rata
                    centroRatas += other.transform.position;

                    //Y la procesamos
                    numRatasProcesado++;
                }
            }
        }
        private void OnTriggerExit(UnityEngine.Collider other)
        {
            //Si sale una rata nueva en el trigger...
            if (other.GetComponent<Merodear>() != null)
            {
                //Se resta el numero de ratas
                numRatas--;

                //Si hay menos ratas...
                if (numRatas < maxRatas)
                {
                    //Entonces deja de huir
                    llegarScript.prioridad = 0;

                }
            }
        }
    }
}
