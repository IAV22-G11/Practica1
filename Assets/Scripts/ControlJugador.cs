/*    
   Copyright (C) 2020 Federico Peinado
   http://www.federicopeinado.com

   Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
   Esta asignatura se imparte en la Facultad de Informática de la Universidad Complutense de Madrid (España).

   Autor: Federico Peinado 
   Contacto: email@federicopeinado.com
*/
namespace UCM.IAV.Movimiento
{

    using UnityEngine;
    using UnityEngine.SceneManagement;

    /// <summary>
    /// Clara para el comportamiento de agente que consiste en ser el jugador
    /// </summary>
    public class ControlJugador : ComportamientoAgente
    {
        //PARA QUE LA FLAUTA SUENE
        private AudioSource source;

        bool tocaFlauta = false;

        private void Start()
        {
            source = GetComponent<AudioSource>();
            source.Stop();
        }
        /// <summary>
        /// Obtiene la dirección
        /// </summary>
        /// <returns></returns>
        public override Direccion GetDireccion()
        {
            Direccion direccion = new Direccion();
            direccion.lineal.x = Input.GetAxis("Horizontal");
            direccion.lineal.z = Input.GetAxis("Vertical");
            direccion.lineal.Normalize();
            direccion.lineal *= agente.aceleracionMax;

            // Podríamos meter una rotación automática en la dirección del movimiento, si quisiéramos

            return direccion;
        }

        private void Update()
        {
            //Si pulsas el espacio
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //Se activa el trigger de la flauta
                tocaFlauta = true;
                source.Play();
            }
            //Si deja de tocarlo...
            else if (Input.GetKeyUp(KeyCode.Space))
            {
                //Se desactiva
                tocaFlauta = false;
                source.Stop();
            }
            //Si pulsas la N...
            if (Input.GetKeyDown(KeyCode.N))
            {
                //Reiniciamos la escena
                SceneManager.LoadScene("Hamelin");
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.GetComponent<Merodear>() != null)
            {
                //Si toca la flauta..
                if (tocaFlauta)
                {
                    //Cambiamos la prioridad para que las ratas se acerquen
                    other.GetComponent<Merodear>().prioridad = 2;
                    other.GetComponent<Llegar>().prioridad = 1;
                }
                else
                {
                    //Cambiamos la prioridad para que las ratas se acerquen
                    other.GetComponent<Merodear>().prioridad = 1;
                    other.GetComponent<Llegar>().prioridad = 2;
                }
            }
        }


    }
}