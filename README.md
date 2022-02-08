# Practica1

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