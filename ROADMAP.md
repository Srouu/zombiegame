# Roadmap du jeu coop zombie déjanté

(Version condensée pour Claude Code. La version complète est dans le document de roadmap en ligne.)

## État actuel

- Phase en cours : **Phase 1 (prototype jouable)** (phase 0 terminée)
- Cases cochées ci-dessous = terminé. Mets ce fichier à jour à la fin de chaque étape.
- Voir `STORY.md` pour l'histoire de l'île 1, les 3 vagues choisies depuis la trappe du bus, le camp modélisé (achat avec « utiliser »), les réparations du bus avec nuage, le boss requin et la cagnotte commune.
- L'inventaire d'armes a 2 cases au départ, +1 toutes les 2 maps, 4 au maximum. Les munitions sont affichées en haut à droite de l'écran, hors inventaire.

## Vision

Coop 1 à 4 joueurs en chaos total. Quatre potes traversent des îles et maps délirantes, le bus tombe en panne, on paie la réparation avec l'argent gagné, puis on repart. Les zombies sont un prétexte pour des armes absurdes, des références partout et des moments clippables en live.
Le mode saboteur est un mode à part, pas la campagne de base.

Trois piliers : le chaos fun (explosions, physique, tir allié non létal), les armes iconiques (hommages originaux), le pari sur les objets.

## Règles de design à respecter

### Structure d'un niveau
Le bus arrive, on survit et on gagne de l'argent, puis on achète l'amélioration obligatoire qui permet de repartir.

| Niveau | Arrivée | Achat de fin de niveau |
| --- | --- | --- |
| 1. Île déserte | Le bus s'échoue | Kit aile + boost pour décoller |
| 2. Nuages et licornes | Le bus s'envole, touché en route | Réparation du bus |
| 3. Arène façon foot-voiture | Le bus fait exploser le dôme, atterrit au centre | À définir |
| 4 et suivants | Panne dans un lieu insolite | Une amélioration du bus par niveau |

Les niveaux terminés restent rejouables depuis le hub (farm de cosmétiques). Une nouvelle map est accessible directement.

### Armes et inventaire
- Tout le monde démarre avec un pistolet.
- Les autres armes se trouvent sur un zombie (un zombie porteur par map) ou cachées sur la map.
- Cases d'inventaire : uniquement pour les armes. 2 cases au départ, +1 toutes les 2 maps, maximum 4.
- Les munitions ne prennent pas de place : elles s'empilent en haut à droite de l'écran.
- Les armes ont des paliers d'amélioration payés avec l'argent du jeu.
- Familles : hommages iconiques (pistolet à physique avec jauge de surchauffe 0 à 100 %, grenade flash abusée qui aveugle aussi l'équipe, bouteille boost, canon à pistache), armes cartoon à tricks (baliste, lance-harpon), non létales (taser, pistolet à propulsion), classiques améliorables (pistolet, pompe, AK, M4), baguettes magiques à sortilèges inventés.
- Surchauffe : à 100 % l'arme est bloquée et moins puissante tant que la jauge n'est pas redescendue.

### Zombies, drops, pari
- Zombies de base : lâchent de l'argent.
- Zombies à thème (ex. zombie années 80) : chance de lâcher un objet rare.
- Objet rare : équipable comme cosmétique avec un petit bonus, ou revendable, ou parié.
- Pari : on jette l'objet à un endroit dédié, animation, résultat de x0,5 à x50 ou destruction. Les probabilités diminuent pour les gros multiplicateurs. Résultat tiré côté hôte.

### Modes
- Campagne coop (mode de base) : 1 à 4 joueurs, tous dans la même équipe.
- Mode saboteur (mode à part) : un joueur piège les autres. Développé après le multijoueur de base.

## Phases

### Phase 0 : mise en route
- [x] Projet Unity 6 (URP) créé et ouvert
- [x] Dépôt Git avec `.gitignore` Unity
- [x] Structure de dossiers : Scripts, Prefabs, Scenes, Art, Audio
- [x] Nom de travail du jeu choisi
Critère de sortie : une scène avec un sol et un cube, lancée en mode Play sans erreur.

### Phase 1 : prototype jouable
- [x] Personnage 3e personne : déplacement, saut, caméra
- [ ] Pistolet qui tire, avec recul
- [ ] Zombie-capsule qui poursuit le joueur et subit des dégâts
- [ ] Mort du zombie : particules et pièces ramassables
- [ ] Baril explosif avec onde de choc
- [ ] Compteur d'argent
- [ ] Taser avec tir allié qui immobilise un coéquipier
Critère de sortie : 10 minutes d'amusement seul dans l'arène.

### Phase 2 : tranche verticale (île déserte, coop locale)
- [ ] Île déserte avec début, objectif et achat de fin de niveau
- [ ] 3 ou 4 types de zombies
- [ ] Inventaire d'armes (2 cases au départ) et munitions en pile à l'écran
- [ ] 5 ou 6 armes à trouver, dont le pistolet à physique avec surchauffe
- [ ] Améliorations d'armes (2 ou 3 paliers)
- [ ] Drops, objets rares et pari
- [ ] Écran de fin de mission
Critère de sortie : des amis rient en jouant ensemble.

### Phase 3 : multijoueur en ligne et saboteur
- [ ] Choix de la solution réseau après un test comparatif
- [ ] Lobby et invitation d'amis
- [ ] Synchronisation joueurs, zombies, tirs, explosions, drops, pari
- [ ] Déconnexions et reprise
- [ ] Mode saboteur (mode à part)
Critère de sortie : une mission complète à 4 en ligne, stable.

### Phase 4 : hub du bus, boutique, casino, progression
- [ ] Le bus et ses transitions
- [ ] Amélioration du bus à chaque niveau
- [ ] Cases d'inventaire gagnées toutes les 2 maps
- [ ] Boutiques et casino (monnaie virtuelle)
- [ ] Retour sur les maps terminées
- [ ] Sauvegarde
Critère de sortie : trois missions enchaînées via le bus, avec sauvegarde.

### Phase 5 : contenu
Maps après l'île déserte : nuages et licornes, arène façon foot-voiture (zombies œufs), cimetière, montagne, jungle, volcan.
Pour chaque map : décors, 2 ou 3 zombies à thème, 1 ou 2 armes, pièges de saboteur, lieu de panne, test à 4.

### Phase 6 : polish et sortie
Tests avec petits streamers, outils pour streamers, optimisation, audio, page Steam, démo, sortie en accès anticipé.
