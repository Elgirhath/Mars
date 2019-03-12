# Mars

### [Kanban](https://elgirhath.kanbantool.com/b/498847#)

### [MindMap](https://coggle.it/diagram/XGbqbnV4-hvJL5IA/t/gra)

## Importowanie modeli z Blendera:

### .blend:
+ Importowane są materiały
+ Nie trzeba eksportować do .fbx
+ Animacje
- Czasem może się coś zepsuć, wtedy należy wyeksportować do .fbx

#### Wymagania:
1. Wszystkie obiekty powinny mieć gotowe wymiary w metrach.
2. Gotowe materiały, z poprawnymi nazwami i przypisaniem.
3. Środek sceny będzie w Unity pivotem.
4. W Unity dobrać poprawną skalę w ustawieniach importu (po zaznaczeniu danego obiektu .blend).
5. Do Unity zaimportowane zostaną wszystkie warstwy z Blendera - pozbyć się ich gdy ich nie chcemy.
6. Jeżeli hierarchia obiektów w Blenderze będzie miała więcej niż jeden poziom - może się źle zaimportować do Unity. W takiej sytuacji konieczna będzie ręczna naprawa skali i/lub eksportowanie do .fbx.
7. Jeżeli to możliwe, dla wszystkich składowych obiektów zastosować jeden materiał. Konieczne do tego może być skorzystanie z darmowej wtyczki do Blendera "Texture Atlas" (swoją drogą polecam).
8. W przypadku gdy obiekty mają różne tekstury należy wypalić ("Bake") tekstury w Blenderze.
9. ~~Włączyć opcję "Pack all into .blend", by Unity pobrało tekstury, lub dodać je ręcznie.~~ Dodać tekstury ręcznie.

### .fbx:
+ Poprawny import
- Nie importowane materiały
- Dodatkowy plik, który swoje waży

#### Wymagania (wszystkie te z sekcji .blend oraz dodatkowo poniższe) :
1. Tu do rozwiązania problemu skali można wykorzystać "!Experimental! Apply Transform" z menu eksportu. Nie działa dla hierarchii wielostopniowej.
2. Dodatkowo w menu eksportu jest suwak i przycisk pomagające przy ustawianiu skali.

#### Plik wrzucamy do wybranego folderu w /Assets. Taki plik nie jest jednak prefabem! Aby stworzyć prefab na podstawie modelu, należy najpierw dodać go do sceny, a potem z hierarchii przeciągnąć go do Assetów.

## Pliki lokalne:
Pliki trzymamy w folderach lokalnych - gdziekolwiek w projekcie można utworzyć folder **\_local**, który jest wpisany w **.gitignore**. Do projektu wrzucamy plik **.blend**, folder **Textures**, zawierający tylko konieczne tekstury oraz ewentualny plik **.fbx** (np. w folderze Models/LogBench można utworzyć folder **\_local**, w nim umieścić cały projekt Blendera, Gimpa, tekstury itp., a do Models/LogBench wrzucamy tylko tekstury i gotowy model).

#### Walidacja:
Pracując nad projektem możemy korzystać z plików lokalnych, jednak może to doprowadzić do sytuacji, gdy zcommitujemy projekt z referencjami do naszych lokalnych plików. Wtedy pozostali współpracownicy otrzymają błędy, które naprawione zostaną dopiero przy następnym commicie. W folderze **Extensions** znajduje się więc skrypt, który pozwala na sprawdzenie obecności referencji do plików lokalnych, poprzez menu **`Assets/Find Local References`**. Po kliknięciu na warning podświetli się obiekt, w którym znajduje się referencja. Można też znaleźć dany plik lokalny i z menu kontekstowego wybrać **`Find references in scene`**.

## Materiały w Unity:
### [Dokumentacja](https://docs.unity3d.com/Manual/StandardShaderMaterialParameters.html)

Warto tu zwrócić uwagę na sekcję **Metallic** i **Smoothness**. Jedna tekstura steruje tam oboma zjawiskami jednocześnie (np. **RGB** oznaczają **Smoothness** a **Alpha** oznacza **Metallic**). Takie zjawisko można osiągnąć np. w Substance Painter, ale także przy zwykłym użyciu Photoshopa/Gimpa. Po prostu dodajemy dwie tekstury oraz wybieramy, by **Metallic** sterował kanałem **Alpha**, a **Smoothness** kanałami **RGB**.
