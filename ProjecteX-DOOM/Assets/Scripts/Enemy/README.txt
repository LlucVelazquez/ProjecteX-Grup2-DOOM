=========================================
 ARQUITECTURA DEL SISTEMA D'ENEMICS
=========================================

El sistema d'enemics segueix els principis SOLID i el patro de
composicio sobre herencia. En lloc de crear una subclasse per a
cada tipus d'enemic, cada comportament es un component independent
que s'afegeix o es treu des de l'inspector de Unity.

Un enemic NO ES un tipus concret, sino un GameObject amb una
combinacio de components que li donen capacitats.


-----------------------------------------
 COMPONENTS
-----------------------------------------

  OBLIGATORIS (sempre presents):
    - EnemyHealth               Gestiona els punts de vida
    - EnemySound                Gestiona els sons
    - TargetDetectionBehaviour  Detecta si el target es visible

  OPCIONALS DE COMPORTAMENT:
    - FollowBehaviour           Persegueix el target
    - RotateToTargetBehaviour   Rota cap al target sense moure's
    - AttackBehaviour           Atac en melee
    - ProjectileFiringBehaviour Dispara projectils

  OPCIONALS D'ANIMACIO:
    - EnemyAnimMovement         Animacio de moviment (via NavMeshAgent)
    - EnemyAnimAttack           Animacio d'atac melee
    - EnemyAnimDeath            Animacio de mort


-----------------------------------------
 SEPARACIO DE RESPONSABILITATS
-----------------------------------------

  QUAN atacar, perseguir o disparar  ->  EnemyController
  COM detectar el target             ->  TargetDetectionBehaviour
  COM moure's cap al target          ->  FollowBehaviour
  COM rotar cap al target            ->  RotateToTargetBehaviour
  COM atacar en melee                ->  AttackBehaviour
  COM disparar projectils            ->  ProjectileFiringBehaviour
  COM sonar                          ->  EnemySound
  COM animar el moviment             ->  EnemyAnimMovement
  COM animar l'atac                  ->  EnemyAnimAttack
  COM animar la mort                 ->  EnemyAnimDeath


-----------------------------------------
 COMUNICACIO ENTRE COMPONENTS (EVENTS)
-----------------------------------------

  Els components no es criden directament entre ells.
  Utilitzen events per desacoblar les responsabilitats:

  AttackBehaviour.OnAttack
    -> EnemySound.PlayAttack()
    -> EnemyAnimAttack.PlayAttack()

  EnemyHealth.OnDamaged
    -> EnemySound.PlayHurt()

  EnemyController.OnEnemyDeath
    -> EnemyAnimDeath.PlayDeath()

  Aixi AttackBehaviour i EnemyHealth no saben res de sons
  ni animacions. Cada component es subscriu als events que
  li pertoquen de forma independent.


-----------------------------------------
 MAQUINA D'ESTATS
-----------------------------------------

  Idle ──► Chase ──► Attack
    ^         |
    └───── Return

  Qualsevol estat ──► Die

  Idle    L'enemic espera. Si detecta el target passa a Chase.
  Chase   Persegueix i dispara. Si arriba a rang melee, Attack.
  Attack  Ataca en melee. Si el target s'allunya, torna a Chase.
  Return  Torna a la posicio inicial. Si detecta el target, Chase.
  Die     Para el moviment, llanca OnEnemyDeath i es desactiva.


-----------------------------------------
 JERARQUIA DE PROJECTILS
-----------------------------------------

  Cada ProjectileFiringBehaviour crea el seu propi contenidor
  buit a l'arrel de la jerarquia en temps d'execucio:

  Scene
  ├── Enemy_Imp
  ├── Enemy_Turret
  ├── Enemy_Imp_Projectiles       <- contenidor de l'Imp
  │   ├── Projectile_0
  │   └── Projectile_1
  └── Enemy_Turret_Projectiles    <- contenidor de la Torreta
      └── Projectile_0

  Cada enemic te el seu propi pool de projectils. Aixi no es
  barregen prefabs de tipus d'enemics diferents.


-----------------------------------------
 TIPUS D'ENEMICS
-----------------------------------------

  Combinant components es creen tipus d'enemics sense cap subclasse:

                        Follow  Rotate  Attack  Projectile  AnimMov  AnimAtk
  Melee                   SI      -       SI       -           SI       SI
  Ranged                  SI      -       -        SI          SI       -
  Melee + Ranged          SI      -       SI       SI          SI       SI
  Torreta                 -       SI      -        SI          -        -

  Tots els tipus tenen sempre: EnemyHealth, EnemySound,
  TargetDetectionBehaviour i EnemyAnimDeath.

  NOTA: Els enemics amb FollowBehaviour no necessiten
  RotateToTargetBehaviour, el follow ja rota l'enemic.
  La torreta no es mou i per aixo necessita el component de rotacio.


-----------------------------------------
 PRINCIPIS SOLID APLICATS
-----------------------------------------

  SRP  Cada component te una unica responsabilitat.
       EnemyHealth nomes gestiona vida, no sap res de sons.
       AttackBehaviour nomes gestiona l'atac, no sap res
       d'animacions ni sons.

  OCP  Nous comportaments (ex. LaserFiringBehaviour) s'afegeixen
       sense modificar EnemyController. Nous sons o animacions
       es subscriuen als events existents sense tocar res.

  ISP  Cap enemic depen de comportaments que no utilitza.
       Una torreta no te FollowBehaviour ni EnemyAnimMovement.

  DIP  Els components depenen d'events i abstraccions, no
       d'implementacions concretes entre ells.