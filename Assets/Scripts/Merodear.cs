using UnityEngine;

namespace UCM.IAV.Movimiento
{

    /// <summary>
    /// Clase para modelar el comportamiento de Merodear
    /// </summary>
    public class Merodear : ComportamientoAgente
    {
        //Temporizador para que gire cada 1/5s
        private float temp = 0;
        private float tempChoque = -10;

        private float rotation;

        //Cambiamos nuestra orientacion aleatoriamente
        int numAleatorio;

        /// <summary>
        /// Obtiene la dirección
        /// </summary>
        /// <returns></returns>
        public override Direccion GetDireccion()
        {

            Direccion result = new Direccion();

            //Sacamos la velocidad a partir de la forma vectorial de la orientacion
            result.lineal = agente.velocidadMax * transform.forward;




            //Si todavia seguimos contando
            if (temp > 0)
            {
                temp -= Time.time;
            }
            //Rotamos al raton
            else
            {


                if (Random.value < 0.5)
                {
                    numAleatorio = -1;
                }
                else
                    numAleatorio = 1;

                rotation = numAleatorio * agente.rotacionMax * Random.value;

                //Ponemos un random entre 1 y 5 a temp
                temp = Random.Range(200, 1000);
            }

            //Con random value hacemos que en vez de rotar X o -X rote tambien en los valores intermedios
            result.angular = rotation;

            tempChoque -= Time.time;

            return result;
        }

        private void OnCollisionEnter(Collision other)
        {
            if (tempChoque < 0)
            {
                //Si el objeto con el que nos hemos chocado tiene la layer de pared...
                if (other.gameObject.layer == 3)
                {
                    //Damos 180º a el raton
                    transform.localEulerAngles = new Vector3(transform.rotation.x, transform.rotation.y +180, transform.rotation.z); // Flipped
                    tempChoque = 2000;
                }
            }
            

        }
    }
}
