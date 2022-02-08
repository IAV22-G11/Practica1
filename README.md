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

**Comportamientos a añadir**

El script de Llegar va a ser implementado igual que en el libro de IA for games de Millington:

class KinematicArrive:
character: Static
target: Static

maxSpeed: float

# The satisfaction radius.
radius: float

# The time to target constant.
timeToTarget: float = 0.25

function getSteering() -> KinematicSteeringOutput:
result = new KinematicSteeringOutput()

# Get the direction to the target.
result.velocity = target.position - character.position

# Check if we’re within radius.
if result.velocity.length() < radius:
	# Request no steering.
	return null

# We need to move to our target, we’d like to
# get there in timeToTarget seconds.
result.velocity /= timeToTarget

# If this is too fast, clip it to the max speed.
if result.velocity.length() > maxSpeed:
	result.velocity.normalize()
	result.velocity *= maxSpeed

# Face in the direction we want to move.
character.orientation = newOrientation(
character.orientation,
result.velocity)

result.rotation = 0
return result

Estamos planteando como pequeño añadido incluir otro radio algo más grande que el introducido por Millington que permita que una vez realizada la llegada por parte del agente este no vuelva a intentar acercarse hasta salir de este nuevo radio, permitiendo un comportamiento mas natural.

El script de Huida es igual que el de Llegar, salvo que result.velocity = character.position - target.position

Estos dos sccripts constituiran la inteligencia artificial del perro, incluyendo otro mecanismo que detecte cuantas ratas estan cerca suya para detectar los momentos en los que tiene que huir y en los que tiene que intentar llegar a su destino.

Para las ratas se utilizará también el comportamiento llegar mientras el jugador este lo suficientemente cerca tocando la flauta. Esta cercanía se determinará comprobando la distancia de la rata al flautista y comprobando si el jugador esta tocando o no.

En cuanto al merodeo errático de las ratas utilizaremos tambien la clase KinematicWander de Millington:

class KinematicWander:
character: Static
maxSpeed: float

# The maximum rotation speed we’d like, probably should be smaller
# than the maximum possible, for a leisurely change in direction.
maxRotation: float

function getSteering() -> KinematicSteeringOutput:
result = new KinematicSteeringOutput()

# Get velocity from the vector form of the orientation.
result.velocity = maxSpeed * character.orientation.asVector()

# Change our orientation randomly.
result.rotation = randomBinomial() * maxRotation

return result
