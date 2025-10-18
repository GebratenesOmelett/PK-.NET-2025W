## Wymagania I/O

#### Program powinien przyjąć źródło danych:
tekst z klawiatury, 2) ścieżkę do pliku, 3) ścieżkę przekazaną jako argument CLI.

##### Teskst z klawiatury

dotnet run --project TextAnalytics.App

![img.png](img.png)

Zapis do pliku result 

![img_1.png](img_1.png)

##### Teskst z pliku

dotnet run --project TextAnalytics.App sample.txt

![img_2.png](img_2.png)

Odczyt z pliku

![img_3.png](img_3.png)

Zapis do pliku result 

![img_4.png](img_4.png)

#### Program powinien przyjąć źródło danych:

#### Zademonstruj aktualizację wersji pakietu (np. Newtonsoft.Json) oraz odświeżenie zależności.

![img_5.png](img_5.png)

![img_6.png](img_6.png)

#### Dodaj krótką notatkę o SemVer i wpływie aktualizacji na kompatybilność API.

Wersjonowanie semantyczne to schemat kodowania wersji, który koduje wersję za pomocą trzyczęściowego numeru wersji (Major, Minor, Patch).

![img_7.png](img_7.png)

Na kompatybilność API ma głównie wpływ MAJOR. On odpowiada za dokonywanie zmian niekompatybilnych z API.

MINOR oraz PATCH nie mają wpływu na kompatybilność z API.

## Struktura

![img_8.png](img_8.png)