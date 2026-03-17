# ProjecteX-Grup2-DOOM

## Build

La build es troba en format .zip en el drive ja que per el seu pes no s'ha pogut pujar al github.

[Build](https://drive.google.com/file/d/1gzWKw9n1ewqvpuGACm2LSYG5Bl7G0w1W/view?usp=drive_link)

## Sistema de Jugador

El jugador es pot moure per un espai 3d en primera persona, per moure's pot caminar o correr.

Hi ha sistema de vida tant per al jugador com pels enemics, i el jugador a més té un sistema de armadura, amb dos tipus diferents d'aquesta.

El jugador compta amb una escopeta per a atacar als enemics que es pot trobar pel mapa. Els enemics ataquen cos a cos, i llencen projectils cap al jugador.

El joc té un checkpoint per a guardar la posició del jugador i en cas de morir o reiniciar, no haver de tornar a començar des de zero.

## Combat i mecàniques bàsiques

El jugador porta una escopeta amb la que pot atacar als enemics que es troba. Aquesta fa un mal d'entre 5 i 15 per perdigó d'escopeta, cada dispar en llença 7, provocant un mal mínim de 35 i un màxim de 105. Els perdigons de l'escopeta a més tenen dispersió, fent que cada dispar sigui diferent. L'escopeta és una arma hitscan.

El joc té 3 tipus d'enemics:
- Soldat: Persegueix al jugador i ataca cos a cos. El soldat té 20 de vida i un mal d'entre 3 i 15.
- Dimoni: Persegueix al jugador, ataca cos a cos i llença projectils. El dimoni té 60 de vida i un mal d'entre 3 i 24.
- Cacodimoni: Mira al jugador i llença múltiples projectils. El cacodimoni té 1000 de vida i un mal d'entre 3 i 24 per projectil.

Tots els enemics que es mouen persegueixen al jugador i quan el perden de vista tornen a la seva posició inicial.

## Triggers i puzzles

Hi ha diferents triggers repartits pel mapa, que actualitzen l'objectiu principal del joc i activen una sala secreta.

El joc té un puzzle de combinació de botons, has de pitjar els botons en l'ordre correcte per a resoldre el puzzle.

## Level design i flux del joc

El joc comença amb una presentació que et permet entrar en context del joc, per després trobarte en una sala buida on només pot anar en una direcció. A mesua que avances pel joc trobaràs enemics que t'atacaràn i et perseguiran, fins arribar a la sala final o t'enfrontaràs a una gran massa d'enemics i a un boss final.

## UI, HUD i menús

La HUD del jugador mostra la seva vida, armadura i la munició de l'arma actual, a més de diferents icones per representar cada element. La HUD també té una secció de objectiu de la missió, on es pot veure l'objectiu actual a seguir.

El joc comença amb el menú principal amb els botons d'iniciar, per començar partida, opcions, per a retocar diferents opcions del joc, i sortir, per a sortir del joc. Durant el joc prement ESC s'obre el menú de pausa en el que es pot continuar jugant, reiniciar el personatge des de l'últim checkpoint, reiniciar el nivell des de zero, entrar al menú d'opcions o sortir del joc. El menú de opcions permet configurar la sensibilitat del ratolí i el volum general, de la música i dels efectes de so.

## Audio i feedback

Hi ha dues músiques que es poden escoltar al llarg del joc, una durant el menú principal i una altra mentre juguem el nivell.

El joc conté molts efectes de so diferents, per exemple, cada enemic té un so diferent per a quan detecta al jugador, quan l'ataca, quan rep mal i quan mor. El jugador també té diferents sons, com quan dispara amb l'escopeta, rep mal, es queda a poca vida, o mor. Els objectes del joc també provoquen sons, com en recollir kits medics, armadura o munició, així com en actualitzar l'objectiu de la missió o descobrir un secret.
