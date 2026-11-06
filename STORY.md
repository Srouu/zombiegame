# Histoire, transitions et boucle d'une île

Document pour Claude Code. Il complète `ROADMAP.md`. Tout ce qui est marqué « à définir » reste ouvert.

**Statut : seule l'île 1 (l'île déserte) est décidée.** La boucle d'une île (3 vagues, boutiques, réparations avec nuage) est valable pour toutes les îles, mais l'histoire des îles suivantes, leurs boss et leurs transitions sont des idées, pas des décisions. Ne construis rien au-delà de l'île 1 sans demander.

## Pitch

Quatre potes font du parachute. Un coup de vent les emporte et ils atterrissent sur une île déserte, à côté d'un vieux bus en piteux état. Pour repartir, ils doivent réparer le bus pièce par pièce, avec l'argent gagné contre les zombies. Chaque île est une étape du voyage.

Pour l'instant, l'histoire se raconte par les décors, les animations et les gags. Pas de dialogues ni de cinématiques longues.

## La boucle d'une île

Sur chaque île, les 3 modes de vague se choisissent depuis le bus : une trappe s'ouvre sur le bus et on y choisit le mode (pas de menu plein écran) :

| Bouton | Difficulté | Récompense |
| --- | --- | --- |
| Vague tranquille | Facile | Petite (à définir) |
| Vague K.O. | Difficile | Moyenne (à définir) |
| Vague boss final « giga K.O. » | Très difficile, avec les mécaniques propres au boss | Grosse, et l'accès à l'étape suivante (à définir) |

**Règle de progression.** Pour passer au monde suivant, il faut valider les trois vagues (tranquille, K.O., boss). Le déblocage passe par deux paiements de réparation du bus, en pièces (le prix exact est à définir) :
1. **Réparation 1** : payer X pièces débloque la vague K.O.
2. **Réparation 2** : payer Y pièces répare le bus et déclenche le boss.
Après le boss, le bus décolle vers le monde suivant.

Les vagues sont rejouables en boucle pour farmer de l'argent si c'est trop dur.

**Objectif d'équilibrage.** Une équipe doit pouvoir avancer en faisant chaque vague une seule fois : les prix des réparations sont calés sur ce que rapportent les vagues, avec un petit surplus.

**L'argent est commun.** Toute l'équipe partage une seule cagnotte. N'importe quel joueur peut dépenser (armes, munitions, améliorations, pari au casino) et chaque dépense réduit ce qui reste pour les réparations. C'est voulu : cela crée des disputes entre coéquipiers et des moments à clipper, et il faut dépenser avec stratégie. Pistes pour renforcer l'effet (à tester) :
- Afficher qui a dépensé quoi (« Bobbin a dépensé 400 »), dans un petit journal ou sous l'étiquette de prix.
- Une jauge de cagnotte commune visible par tous, avec les deux paliers de réparation marqués.
- Un marqueur ou un ping pour dire « on garde pour la réparation ».
Les paris au casino dépensent aussi la cagnotte commune.

**Un petit camp modélisé, pas de menus.** Chaque île a un camp avec des props 3D. Tout s'achète en jeu : le joueur s'approche de l'objet, une étiquette de prix flotte au-dessus, et il appuie sur « utiliser » pour acheter. Pas d'écran de boutique.
- Un présentoir ou des caisses avec les armes modélisées dessus (boutique d'armes).
- Un établi pour les améliorations (armes, inventaire).
- Une caisse de munitions.
- L'autel du casino (le pari sur les objets rares).
- Les pièces du bus à réparer posées près du bus, chacune avec son prix.
- La trappe du bus, avec trois leviers ou boutons, pour lancer les vagues.

Seuls restent à l'écran le HUD (argent, munitions, armes, équipe), les étiquettes de prix et un message d'achat très court. Le HUD doit rester petit et lisible.

Les objets rares tombent des zombies à thème (cosmétiques avec petit bonus, ou revente, ou pari). Il y a un gros boss par map.

L'argent sert à tout : acheter des armes, s'améliorer, parier, et surtout payer les réparations du bus.

## Les réparations du bus et les nuages

Réparer le bus coûte de l'argent, pièce par pièce. Pour passer d'un état du bus à l'autre sans produire une animation de réparation à chaque fois, un gros nuage cartoon (style gros nuage de poussière de bagarre) recouvre le bus, le temps de changer son modèle. Ce nuage sert de transition pour tous les changements d'état du bus.

États du bus (voir la feuille `bus.png`) : intact, en panne, avec le kit d'ailes et le booster.
Les états intermédiaires (une pièce réparée à la fois) sont à définir.

## Île 1 : l'île déserte

1. **Introduction** : parachute, coup de vent, atterrissage près de l'épave du bus.
2. **Étape 1, réparations** : on gagne de l'argent avec les vagues, et on paie les pièces du bus (liste à définir : roue, pare-brise, moteur, etc.). Chaque réparation passe par un nuage.
3. **Étape 2, le boss** : un énorme zombie requin sort de l'eau. Ses ailerons deviennent les ailes du bus. Les ailes du bus sont bleu marine (#29324D) avec des chevrons roses (#FF4FA3), comme les ailerons du requin (feuille `boss-shark.png`), et non jaunes comme sur la feuille `bus.png`. On réutilise le modèle des ailerons du requin, réduit à environ 40 % de sa taille.
4. **Décollage** : le bus accélère et part dans le ciel, direction le monde des nuages et des licornes (île 2).

## Les boss (idées, à définir)

Un boss par map, chacun avec des mécaniques propres. Piste simple : le boss est la version géante du zombie signature de la map.

| Map | Boss (idée) | Mécanique (idée) |
| --- | --- | --- |
| Île déserte | Zombie requin géant | Surgit de l'eau par vagues, saute et retombe en créant une onde de choc, il faut viser ses ailerons |
| Nuages et licornes | Licorne géante | À définir |
| Arène façon foot-voiture | Œuf géant | À définir |
| Cimetière | Squelette géant | À définir |
| Montagne | Yéti géant | À définir |
| Jungle | Singe géant | À définir |
| Volcan | Golem de lave | À définir |

## Pour Claude Code

- Les 3 boutons de vague utilisent le même système de vagues, avec des paramètres différents (nombre de zombies, types, vitesse, récompense). Données dans des ScriptableObjects.
- Chaque boss est un ensemble de données et de comportements séparé, pour pouvoir en ajouter un par map sans casser les autres.
- Le nuage de transition : effet de particules plus un fondu du bus, à réutiliser pour tous les changements d'état du bus.
- Les récompenses et prix sont à équilibrer plus tard, après les tests avec des amis.

## À définir plus tard

- Le nom du jeu et le ton exact des gags.
- La liste exacte des pièces du bus et leurs prix.
- Les mécaniques détaillées de chaque boss.
- Les autres destinations, la fin du voyage et les dialogues.
