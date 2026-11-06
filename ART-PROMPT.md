# ART-PROMPT : prompts pour générer les visuels du jeu (Perplexity)

À garder dans le dossier `Art`. Tout nouveau visuel (arme, zombie, skin, cosmétique, décor) passe par ce fichier pour rester dans le même style.

## Méthode

1. Ouvre Perplexity et joins toujours `style-guide-v1.png`.
2. Joins aussi la feuille la plus proche de ce que tu veux (voir la liste ci-dessous).
3. Colle le **bloc de style** en entier, puis remplace la ligne `NEW REQUEST` par ta demande.
4. Ouvre l'image en plein écran avant de faire la capture (sinon elle est floue ou assombrie).
5. Vérifie : même style que Wobble, silhouette différente des autres, simple à reconstruire.
6. Sauvegarde dans `Art` avec un nom clair (`weapons-2.png`, `zombies-3.png`). Les anciennes versions vont dans `Art/old/`.
7. Dans Claude Code, donne l'image et le nom de l'élément pour qu'il le construise dans Unity.

## Fichiers de référence

| Fichier | Contenu | À joindre pour |
| --- | --- | --- |
| `style-guide-v1.png` | Wobble, Panic Plunger, Island Palm, palette | Toujours |
| `survivors-bases.png` | Les 4 survivants en T-pose | Personnages, échelle des mains |
| `survivors-faces.png` | Visages et 4 teintes de peau | Visages, peau |
| `accessories.png` | Chapeaux et objets avec points d'attache | Cosmétiques |
| `zombies-1.png` | Prism Hoof, Scramble, Slope Lump, Wobble + butin | Nouveaux zombies |
| `zombies-2.png` | Neon Noise, Dash, Fizzbomb, Rattle Groove | Nouveaux zombies |
| `death-effects.png` | Morts de chaque zombie | Effets de mort |
| `effects-sheet.png` | Explosions, morceaux, taches | Effets, sang |
| `weapons-1/2/3.png` | Armes (à ajouter) | Armes et skins d'armes |

## Bloc de style (à coller en tête de chaque demande)

```
You are a concept artist and art director for an indie game. I attach the approved style guide (Wobble, Panic Plunger, Island Palm) and the approved reference sheet closest to this request.

GAME CONTEXT
Chaotic co-op zombie party game, 1 to 4 players, 3D, funny and easy to clip on stream. Comedic and over the top, cartoon violence with explosions and cartoon splatter allowed.

STYLE
Naive low-poly 3D, chunky oversized shapes, flat-shaded with clearly visible facets, matte flat colors, goofy proportions. NOT smooth, NOT clay, NOT glossy, no blur, no glow haze. Light-gray background (#E8E9ED), soft even lighting. Original designs only: no existing characters, brands, logos, labels or real people.

PALETTE
#E8E9ED, #29324D, #72D6AE, #A58BE8, #FF7869, #FFD65A, #FFF1D0, #59BCD5, #FFFFFF, #FF9F1C, #FF4FA3, plus red #E5384B for blood. Green (#72D6AE) is reserved for zombies.

RULES
1. Each new item must have a silhouette clearly different from the existing ones.
2. Chunky simple shapes, rebuildable in under 3,000 triangles for characters and 1,500 for weapons.
3. Show front and 3/4 views, same scale, plus a black silhouette row.
4. Clean sharp image, no blur, no dark overlay.

NEW REQUEST
[décris ici ce que tu veux]

OUTPUT
For each item give: name, silhouette in one line, main palette as hex codes, 3 key features, and 2 animation ideas.
```

## Modèles de demande (à mettre à la place de `NEW REQUEST`)

### Nouvelle arme
```
Design a new weapon: [nom et idée]. Show it held by a survivor's mitten hand from the attached survivor bases for scale. Mark two attachment points: GRIP and MUZZLE (red dots with labels). Add a simple firing effect in palette colors. Its dominant color and silhouette must differ from all weapons on the attached weapon sheets.
```

### Skin d'arme ou de personnage
```
Keep the exact same shapes as the attached item. Only change the colors and add one pattern or accent. Create 3 skins: [thème 1], [thème 2], [thème 3]. Same pose, same scale, same labels.
```

### Nouveau zombie
```
Design a new zombie: [nom et idée]. Same chunky proportions as Wobble, a simple face with a few flat shapes. Show front, 3/4, idle and attack views, plus a death strip of 5 frames (hit, swelling, burst, flying chunks, ground splat) in the zombie's own colors. Its silhouette must be clearly different from all zombies on the attached sheets.
```

### Nouveau cosmétique
```
Design [N] new cosmetic items for the survivors: [liste]. Chunky simple shapes, same scale as the attached accessories sheet. Mark the attachment socket for each: HEAD, FACE_UPPER, FACE_LOWER, BACK or NECK (red dot with label). Show front and 3/4 views.
```

### Nouvelle map (décor)
```
Create environment key art for a new map: [nom et idée]. Show a wide shot, then 5 separate props from the scene (chunky, same style as the Island Palm), each in front and 3/4 views, plus a palette of 6 colors for this map taken from the project palette. List the zombies and weapons that could belong to it.
```

## Règles à ne pas oublier

- Pas de marque réelle, pas de personne réelle, pas de nom ni de design exact d'un autre jeu. Les références sont des clins d'œil avec des noms et des designs originaux.
- Le vert (#72D6AE) est réservé aux zombies : jamais sur les survivants.
- Les skins ne changent que les couleurs, jamais les formes.
- Le sang reste en cartoon plat, sans organes ni blessures réalistes.

## Prompts en attente

### Fiches techniques des personnages (`model-survivors.png`, `model-zombies.png`)

À joindre : `style-guide-v1.png`, `survivors-bases.png`, `survivors-faces.png`, `accessories.png`, `zombies-1.png`, `zombies-2.png`. Pas urgent : on peut construire les premières versions sans.

```
You are a technical character artist for an indie game. I attach the approved style guide (Wobble, Panic Plunger, Island Palm), the approved survivor bases, survivor faces and accessories sheets, and the two approved zombie sheets.

GOAL
Produce strict ORTHOGRAPHIC MODEL SHEETS so that the characters can be rebuilt as low-poly 3D models and rigged. Do NOT redesign anything: keep the approved designs, proportions, colors and face styles exactly as in the attached sheets.

STYLE
Same naive low-poly look as the approved sheets: chunky simple shapes, flat-shaded with clearly visible facets, matte flat colors. NOT smooth, NOT clay, NOT glossy, no blur, no glow haze. Light-gray background (#E8E9ED), flat even lighting, NO perspective, NO shadows.

PALETTE
#E8E9ED, #29324D, #72D6AE, #A58BE8, #FF7869, #FFD65A, #FFF1D0, #59BCD5, #FFFFFF, #FF9F1C, #FF4FA3, plus the four skin tones #EFC6A5, #D6A074, #B77A52, #6B4032.

POSE AND LAYOUT
- Humanoids in a neutral T-pose, arms straight out, palms down, feet slightly apart.
- Four views for each character, aligned on the same horizontal guidelines and at the same scale: FRONT, SIDE (left), BACK, TOP.
- Draw thin guidelines at the top of the head, the shoulders, the elbows, the waist, the knees and the soles, and mark the total height in head units (for example "6 heads tall").
- Mark the joints for rigging with small blue dots and labels: neck, shoulders, elbows, wrists, hips, knees, ankles, and the spine at 2 points.
- Mark the attachment sockets with red dots and labels: HEAD (for hats), FACE_UPPER (for goggles), FACE_LOWER (for a moustache), NECK (for a scarf), BACK (for a backpack), HAND_R_GRIP (where a weapon is held in the right hand, with the direction of the barrel), and HAND_L_SUPPORT (the left hand support position).
- Show an EXPLODED PARTS view on the side: head, torso, upper arm, forearm, hand, thigh, shin, boot, plus clothing pieces, as separate shapes so that each can be a separate mesh.
- Show the FACE as a flat texture with the neutral expression, separate from the head.
- State a triangle budget for each character under 3,000.

SHEET 1: SURVIVORS (base versions, no hat, no accessories)
1. Beacon (yellow raincoat, stops above the knees), 2. Bobbin (round puffy red suit), 3. Pebble (orange coverall), 4. Stretch (tall and lanky, blue). Use the neutral expression on the faces and the light skin tone. Show the 4 heads next to each other with their 4 skin tones as a color swatch row.

SHEET 2: ZOMBIES OF THE FIRST ISLAND
1. Wobble (basic zombie), 2. Neon Noise (80s zombie with boombox), 3. Dash (fast zombie), 4. Fizzbomb (explosive zombie). Mark where the loot crate is attached on the back for Wobble, and where the fuse is for Fizzbomb.

OUTPUT
Two clean, sharp sheets with correct labels. For each character give: name, height in head units, a list of the separate parts, the joint list, the socket list, the main palette as hex codes and 2 animation ideas.
```
