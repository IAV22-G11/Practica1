# Practica1

**Resumen del enunciado**

La practica 1 consiste en crear un modelo en el que controlas a un flautista, el cual está rodeado de personajes inteligentes que tenemos que programar. Tenemos un perro que
nos tiene que seguir a toda costa, y varias ratas pululando en un escenario limitado, las cuales tienen varios comportamientos
en funcion de lo que el jugador haga. Si el jugador toca la flauta, las ratas que estén relativamente cerca se aproximarán y nos seguirán en todo momento. Mientras no toquemos 
la flauta tendran un movimiento aleatorio. Sin embargo si el perro tiene muchas ratas cercas se alejarán de ellas sin importar lo lejos que llegue a estar de nosotros. Creando así
un comportamiento de "bandada".

El objetivo es crear un entorno definido con varios obstáculos, las ratas, el perro y nosotros. Una vez las ratas lleguen a estar lo más próximas posible a nosotros se colocarán
de manera ordenada a modo de manada que nos seguirá hasta donde vayamos.

Hay que diseñar el comportamiento adecuado para cada individuo y que esto mismo sea plasmado en un documento que hay que defender posteriormente al profesor.

La escena de unity debe de contener:
-Un entorno con obstáculos con el avatar que controlamos, el perro y las ratas.
-Plasmar el comportamiento del perro, tanto el de persecución del avatar como el de huida de las ratas cuando hay varias cerca.
-Plasmar el comportamiento de las ratas, tanto el movimiento errático cuando no estamos tocando hasta el desplazamiento en bandada para llegar al flautista.

Cabe mencionar la limpieza del código y organización de la práctica (commits frecuentes, codigo limpio, etc)

Restricciones de la practica:
-Utilizar unicamente unity sin reutilizar codigo de ningun lado (Los plugins son opcionales siempre y cuando se mencione al profesor).
-Documentar correctamente las eurísticas, algoritmos y estrategias utilizadas.
-Diseñar lo más eficiente y limpio posible.

Se recomienda crear una "consola de trucos" para poder testear comodamente las escenas con teclas rapidas para poder hacer todas las pruebas necesarias más facilmente

Actividades opcionales:
Colocar los obstáculos de manera estratégica usando algoritmos de ruido o secuencias.
-Añadir más de un flautista para que compitan por atraer a las ratas.
-Añadir percepcion al perro para evadir a las ratas manteniendose cerca del avatar.
-Mejorar el comportamiento de las ratas y el perro para que sean capaces de evitar correctamente los obstáculos.

**Punto de partida**

En la plantilla aportada se cuenta con una escena de Unity con distintos elementos que ya pueden ser utilizados para los personajes de la práctica y la creación del escenario.
En cuanto a los comportamientos de IA que es la parte que nos incumbe para esta asignatura se aporta una cierta infraestructura que se ha de mantener y ampliar.

Esta infraestructura tiene su base en el componente script Agente, este script tiene como objetivo guardar toda la información relevante para un objeto o personaje de la escena.
Desde guardar la velocidad, orientación... como irla actualizando gracias a las funciones de Unity Update y FixedUpdate.
**NOTA:** para la realización de la práctica hemos cambiado parte de los métodos aportados por lo que nosotros consideramos un mal uso o quizás un uso desactualizado del componente
RigidBody de Unity. Este componente tiene una propiedad llamada IsKinematic que permite determinar si un objeto físico es dinámico o cinemático y actuar en consecuencia. Por ello
en lugares en los que antiguamente se comprobaba si el GameObject cuenta con un RigidBody se va a comprobar además si este es dinámico o cinemático.

Después nos encontramos con el componente script ComportamientoAgente, este script es una clase abstracta que marca la base para todos los comportamientos a implementar en esta práctica,
todos los nuevos comportamientos deberán de heredar de ComportamientoAgente.

Otro script destacable es Direccion, este script cuenta simplemente con una clase Direccion que será utilizada en otros scripts para manejar de forma cómoda el output de muchas de nuestras
funciones de IA.

El otro script destacable que nos queda por comentar es el de ControlJugador que hereda de ComportamientoAgente y permite el control del flautista por parte del jugador.

**Solución implementada**

AI for games Third Edition by Ian Millington

El script de Llegar, utilizado tanto por el perro como por las ratas emplea un algoritmo explicado en el libro AI for games Third Edition de Ian Millington, el cuál es el principal referente empleado en esta práctica.

El código de este script realizará un acercamiento del agente si este se encuentra demasiado lejos del objetivo, sino no se realizará movimiento por parte de este script.

Comprobación de distancia realizada:
 # Check if we’re within radius.
if result.velocity.length() < radius:
	# Request no steering.
	return null

Obtención del movimiento y acercamiento:
 # Get the direction to the target.
result.velocity = target.position - character.position

El script de Huida es igual que el de Llegar, salvo que result.velocity = character.position - target.position

Estos dos sccripts constituyen la inteligencia artificial del perro, incluyendo otro mecanismo que detecta cuantas ratas estan cerca suya para detectar los momentos en 
los que tiene que huir y en los que tiene que intentar llegar a su destino. Por otra parte se tiene en cuenta cuando el perro empieza a huir para que no empiece a perseguir 
justo después y realice un comportamiento extraño en el cual parece que está en continuo movimiento errático.

Para las ratas se utiliza también el comportamiento llegar mientras el jugador esté lo suficientemente cerca tocando la flauta. Esta cercanía se determinará comprobando la 
distancia de la rata al flautista y comprobando si el jugador esta tocando o no. Además, se emplea otro script Separación que previene que las ratas se junten demasiado entre 
si cuando estan persiguiendo al flautista. Este último comportamiento también está inspirado en el libro de Millington.

Este comportamiento se encarga de mantener esa distancia entre ratas, siendo direction la distancia que hay entre la rata mas cercana y la rata en cuestión:

if distance < threshold:
    # Calculate the strength of repulsion
    # here using the inverse square law
      strength = Mathf.Min(
      decayCoefficient / (distance * distance),
      agente.aceleracionMax);

    # Add the acceleration.
      direction.Normalize();
      result.lineal += strength * direction;

En cuanto al merodeo errático de las ratas utilizamos la clase KinematicWander de Millington pero con algunas variaciones:

Este código permite que las ratas se muevan de manera aleatoria. Las ratas se mueven hacia delante mientras no hayan chocado contra una pared. Durante este movimiento 
utilizamos un contador temp que controla que las ratas no tengan un objetivo aleatorio de rotacion todo el rato, sino que solo decidan tomar una nueva rotacion despues 
de unos segundos:

 # Sacamos la velocidad a partir de la forma vectorial de la orientacion
   result.lineal = agente.velocidadMax * transform.forward;

 # Si todavia seguimos contando
   if temp > 0
      temp -= Time.time;
   # Rotamos al raton
   else
   		if Random.value < 0.5
           	numAleatorio = -1;
     	else
            numAleatorio = 1;

        rotation = numAleatorio * agente.rotacionMax * Random.value;

      # Ponemos un random entre 1 y 5 a temp
        temp = Random.Range(200, 1000);

 # Con random value hacemos que en vez de rotar X o -X rote tambien en los valores intermedios
   result.angular = rotation;

   tempChoque -= Time.time;

Si la rata se choca contra una pared, esta dejará de moverse aleatoriamente mientras se encuentre en colisión y en vez de eso procederá a alejarse de la pared con la que ha colisionado:

result.lineal = transform.position - other.transform.position;
result.lineal.Normalize();
result.lineal *= agente.aceleracionMax;
 # Damos 180º a el raton
 # transform.localEulerAngles = new Vector3(transform.rotation.x, transform.rotation.y + 180, transform.rotation.z); // Flipped
 # agente.velocidad *= -1;
 # agente.rotacion += 180;
tempChoque = 2000;

Modificamos el kinematicWander para que en vez de escoger entre izquierda o derecha pudiera también escoger entre todo el rango intermedio, así el movimiento luce más aleatorio
de deambular. También para ser más eficientes hicimos los choques con la pared mediante capas de colisiones para que no lo comprobara cada vez que se choquen con otras ratas o el perro.

Como debilidades principales esta implementación presenta un comportamiento un tanto dudoso en las ratas que al llegar a su objetivo de persecución, 
con tal de mantenerse lo más cerca posible del jugador y mantener cierta distancia, estas siempre se encuentran en movimiento oscilando entre la persecución y la separación.
Si quisieramos dejar a un lado esta optimalidad en cuanto a distancia para hacer un comportamiento más realista podríamos implementar un cierto radio de margen en el cual no 
resume la persecución hasta que no salga de él. Decidimos no tomar esta via ya que para el propósito de esta prática preferiamos un comportamiento más crudo y óptimo a uno mas natural.