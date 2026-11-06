# Projet : jeu coop zombie déjanté (nom de travail)

Jeu coop 1 à 4 joueurs en chaos total, pensé pour les streamers (esprit How to Fish : 3D low-poly naïf, humour absurde).
Moteur : Unity 6 (URP), langage C#, développement sous Windows.
Nom de travail : ZOMBIEGAME (provisoire, à changer avant la page Steam).
Équipe : 3 personnes (Arthur + 2 amis), débutantes sur Unity. Tu es le développeur principal du code.

## Au début de chaque session

1. Lis `ROADMAP.md` (vision, règles de design, phases) et `STORY.md` (histoire de l'île 1, boucle d'une île, camp, boss, cagnotte commune). Seule l'île 1 est décidée.
   Les images de référence validées sont dans le dossier `Art/` (voir le tableau de `ART-PROMPT.md`). Regarde l'image concernée avant de construire un modèle, un décor ou une interface.
2. Repère la phase en cours dans la section « État actuel » de `ROADMAP.md` et ne saute pas de phase.
3. Réponds en français.

## Règles de travail

- Fais de petits changements testables. Après chaque changement, dis comment le tester dans Unity (quelle scène, quel bouton Play, ce qu'on doit voir).
- Quand une action manuelle dans l'éditeur Unity est nécessaire (placer un objet, brancher une référence dans l'inspecteur), explique-la pas à pas, avec les noms exacts des menus.
- Ne supprime aucun fichier sans demander. Propose des commits Git fréquents avec un message clair.
- Écris le code en pensant au multijoueur dès le départ : logique de jeu séparée de l'affichage, état centralisé, résultats aléatoires (pari, drops) tirés côté hôte.
- Style visuel : low-poly naïf, formes simples (capsules, cubes, sphères), couleurs franches.
- Aucun asset ni nom protégé : pas de marque réelle, pas de personne réelle, pas de nom ou design exact d'un autre jeu. Les références sont des clins d'œil avec des noms et des designs originaux.
- Monnaie 100 % virtuelle : jamais d'argent réel lié aux paris ou aux objets.

## Conventions de code

- Scripts dans `Assets/Scripts/` rangés par thème (Player, Enemies, Weapons, Economy, UI, Core).
- Classes et méthodes en PascalCase, champs privés en `_camelCase`.
- Champs réglables dans l'inspecteur avec `[SerializeField] private`.
- Données de gameplay (armes, zombies, objets) dans des ScriptableObjects plutôt qu'en dur dans le code.
