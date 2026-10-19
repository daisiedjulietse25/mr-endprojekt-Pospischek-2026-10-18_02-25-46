# Mixed Reality Eigenstudium E+F: Endprojekt Planetly

## 1. Ziel des Projekts
Projekt von Julia Pospischek, se25m050

Es soll eine VR Applikation namens Planetly umgesetzt werden, die dem Konzept der ersten Abgabe (siehe EigenstudiumA_B_Konzept_Pospischek_improved.pdf) entspricht. Ziel ist es, das Sonnensystem spielerisch zu vermitteln, Informationen zu den Planeten zu geben und dabei 6-DoF zu ermöglichen. Neben der Applikation und einer Live-Demo (bzw. Aufnahme der Demo) soll eine PowerPoint-Presentation (siehe Praesentation.pdf) erstellt werden. 

## 2. Aufsetzen des Projekts

### 2.1 Hardware

In der Meta Horizon App konnte ich die Quest 2 mit meinem Acount koppeln, ein Dev Team erstellen und den Entwicklermodus auf dem Headset aktivieren. 

### 2.2 Software
 
In Unity 6.6 habe ich ein Universal 3D Projekt mit URP erstellt und das OpenXR Plugin unter Unity Registry installiert. Als nächsten Schritt habe ich ein Build Profile unter XR/Meta eingestellt und die Meta XR All-in-One SDK gemeinsam mit OpenXR dafür ausgewählt. 

Aufgrund der Erfahrung durch die 1. Übung wurde erneut das Asset "Planets of the Solar System 3D" für die Planeten verwendet (https://assetstore.unity.com/packages/3d/environments/planets-of-the-solar-system-3d-90219). Das Asset wurde über Window/Package Manager in das Projekt importiert.

Dann habe ich Window/Meta/Tools/Buidling Blocks verwendet, um wie in der LV hergezeigt, die notwendigen Building Blocks hinzuzufügen. Ich habe als erstes die Main Camera entfernt und Camera Rig hinzugefügt. Dann habe ich noch zusätzlich Hand Tracking ausgewählt.

## 3.1 Umsetzung: Planeten, Umgebung und Canvas

Nach dem Setup in Schritt 2 konnte nun die Umsetzung beginnen. Für die graphische Darstellungen wurde zuerst das Skybox Material, das im Asset Package enthalten ist, dem Environment unter Window/Rendering/Lighting als Skybox Material eingestellt. Dann habe ich die Planeten-prefab Dateien aus dem Asset-Package in die Szene gezogen und in einer geraden Linie nacheinander angeordnet. Die Größen der Planeten skalieren von 1 bis 0.65 in 0.05 Schritten basierend auf ihrem Durchmesser, die Sonne hat einen größeren Scalewert. 

![alt text](Bild1_Reihe.PNG)
Bild 1: Blick links von User nach Start von Planetly. Die Planeten sind in einer Linie aufgereit und darüber ist ein Canvas zu sehen. 

Dann habe ich noch ein UI Canvas erstellt, der den User willkommen heißt und später die Möglichkeit bieten wird zu dem 2. realistischen Modus des Sonnensystems zuwechseln. Da dieser Canvas auch für Anweisungen genutzt wird, wird der Textteil später referenziert. 

![alt text](Bild2_Reihe.PNG)
Bild 2: Blick rechts von User nach Start von Planetly. Die Planeten sind weiterhin in einer Linie aufgereit und darüber ist ein Canvas zu sehen. Ganz rechts sieht man die Sonne. 

## 3.2 Umsetzung: Auswahl der Planeten und Anzeige von Detailansicht

Damit die Kugeln der Planeten auswählbar sind, habe ich aus den Meta Building Blocks einen Interaction Rig der Szene hinzugefügt. Dies stellt sicher, dass Hand Ray Interactors von den getrackten Händen ausgehend angezeigt werden. 

Dann habe ich das Objekt ModeManager erstellt und diesem das Script PlanetDetailMode.cs hinzugefügt, in dem die Logik liegt. Dann habe ich für die Planeten einen Sphere Collider sowie eine GameObject InteractionSurface erstellt und diesem das Script Collider Surface angehängt. Die Collider Surface enthält den Sphere Collider der Erde und das Erdenobject bekommt die Komponente Ray Interactable, welche mit dem neuen InteractionSurface Objekt verbunden ist.

Damit die Auswahl der Planeten mit der Pinch Aktion getriggert wird, habe ich den Planeten die Komponente Interactable Unity Event Wrapper hinzugefügt. Unter der Variable Ray Interactable ist das Ray Interabltable Script vermerkt. Zuletzt musste ich in den Visuals des Rays einstellen, dass er nicht versteckt wird, wenn gerade keine Interaktion möglich ist. 

Die Detailansichten der Planeten habe ich mit dem Planeten-prefab und einem Canvas jeweils umgesetzt in einzelnen Objekten. In den Canvas Objekten habe ich einen Offset für den Content von 30 auf allen Seiten eingestellt und dann die Informationen in eigenen Objekten gespeichert und gestylt. Dann habe ich noch einen Button eingefügt, damit man wieder zurück zum Sonnensystem gehen kann. Das hat dann dazu geführt, dass ich eine neue Szene erstellt habe: eine Szene für das ganze Sonnensystem und jeweils Szenen für die Detailansichten mit einem Planeten und dessen Informationen. So kann nun über die Pinch Interaktion mit den Planeten und dann den Zurück Button über das Objekt ModeManager zwischen den Szenen gewechselt werden. Damit der Canvas, der den Button enthält, die Rays erkennen kann, habe ich eine RayCanvasInteraction hinzugefügt und sichergestellt, dass die Komponenten Ray Interactable und Pointable Canvas sowie die Einstellung Raycast Target aktiv waren. 

![alt text](Detailansicht.PNG)
Bild 3: Die Detailansicht eines Planeten mit dem Himmelskörper rechts und den Informationen zu dem Planeten links. Es gibt 2 Buttons: einem für ein Zurück zum Sonnensystem und einen für den Vergleich mit einem anderen Planeten.

Eine der Anforderungen, die ich gestellt habe ist, dass neben den Informationen zum Planeten in der Detailansicht auch der Planet selbst rotiert werden kann. Dafür habe ich in der Detailansichtsszene den Planeten einen RigidBody gegeben, der kein Gravity verwendet, kinematic ist und nicht bei einem unselect geworfen wird, damit er an der selben Stelle bleibt. Außerdem hat er das Script Grabbable, Hand Grab Interactable und One Grab Rotate Transformer. 

Um die Anforderung, dass die Sonne und Gestein-, Eis- und Gasplaneten unterschiedliche Soundeffekte haben, zu erfüllen, habe ich das Skript PlanetTypeAudio.cs erstellt und Free Asset Packages mit Soundeffekten im Package Manager heruntergeladen und aktiviert. So habe ich nun in der Detailansicht einen bestimmten Sound basierend auf dem Planetentypen und in der Szene mit dem Sonnensystem einen durchgehenden Sound. Das aktivieren der in der Summe 4 AudioSources passiert in den Funktionen, die die Szenen jeweils aufruft und beendet.

## 3.3 Vergleich der Planeten

Um Planeten miteinander zu vergleichen wurde in der Detailansicht unter dem Zurück Button ein Vergleich Button eingefügt. Dieser ruft in dem selben Script auf das der Zurück-Button zugreift, eine Comparision Funktion auf. 

Dann habe ich ein neues Objekt ComparisonMode erstellt, dass die beiden Planeten mit ihren Informationen und einem Devider in der Mitte enthält. Zusätzlich setze ich so im ModeManager die 2 Buttons zum zurückgehen und vergleichen für alle 8 Planeten plus die Sonne auf inaktiv, wenn man sich im ComparisonMode aufhält. 

![alt text](Vergleich.PNG)
Bild 4: Interface für Vergleich von 2 Planeten mit einem Button für ein Zurück zum Sonnensystem

In PlanetDetailMode.cs werden dann die Objekte für die Linke und Rechte Seite mit den schon vorhandenen Detailansichten befüllt. Das Audio ist in diesem Fall auf eine einzige Hintergrundmusik eingestellt, da man vielleicht unterschiedliche Typen miteinander vergleicht. Zur Auswahl des Planetens, mit dem man vergleichen möchte, wurde einfach das Sonnensystem wiederverwendet, wobei eine Auswahl des selben Planetens nicht möglich ist. Wie schon zuvor erwähnt, wird der Canvas für Instruktionen verwendet. 

![alt text](Select_2nd_Planet.PNG)
Bild 5: Auswahl des 2. Planeten für Vergleich

## 3.4 Umlaufbahnen

Für die Simulation der Umlaufbahnen habe ich mein Skript für die 1. Übung abgeändert und zu RealistcPlanet.cs und RealisticSolarSystemController.cs weiterentwickelt. Die Canvas mit einem Counter für die Tage des Kalenderjahrs und der Pause/Playbutton als neue Funktionen im Objekt RealisticUI eingebaut. Im Objekt RealisticSolarSystem befinden sich die prefabs der Planeten mit dem Script RealisticPlanet.cs aber ohne Interaktionskomponenten. So werden die Planeten nun nicht mit einem gleichmäßigen symbolischen Abstand angezeigt sondern in ihrer skalierten richtigen Größe und dem richtigen Abstand wie in der Tabelle in Angabe-UE-Unity.pdf beschrieben (beide multipliziert mit dem Faktor 10^-3). Die Sonne hat im Scale eingestellt ihren Radius aus der Angabe Mal 2 mit dem Faktor multipliziert. Damit die Dimensionen der Abstände sichtbar werden, wird in RealisticSolarSystemController.cs ein Wert von 0.001 als Displayscale verwendet. Das bedeutet aber auch, dass die Planeten sehr sehr klein sind, was wiederrum aber maßstabgetreu ist. 

![alt text](IMG_1157.PNG)
Bild 6: Realistischer Modus mit 3 Canvas, die links eine Zurückmöglichkeit zum symbolischen Modus bietet, in der Mitte den Kalender anzeigt mit den Tagen des Erdenjahres und dem Pausebutton und rechts die Geschwindigkeit der Simulation anzeigt. Darunter sieht man das Sonnensystem mit den Planeten und ihren gelben Umlaufbahnen. 

Für den Wechsel zwischen dem realistischen und symbolischen Modus habe ich dann dem Objekt RealisticModeManager das Skript RealisticSolarSystemController.cs angehängt. Dieser wird dann im SolarSystemMode dem Button als OnClick Event übergeben und startet den Realistic Mode. Das selbe wird mit TogglePause() bei dem PlayPauseButton und CloseRealisticMode() beim CangeToSymbolicButton gemacht, weil dies Funktionen sind, die den Text in den Buttons ändern und ihre Funktionalität beschreiben. Auch diese Canvas Objekte wurden durch das selbe Vorgehen wie in Abschnitt 3.2 interagierbar gemacht.

Der letzte Schritt war dann das Verstellen der Geschwindigkeit der Simulation durch die Drehung der Hand. Dafür habe ich im UI die Geschwindigkeit anzeigen lassen und durch die linke Hand mit dem Skript LeftHandSpeedControl.cs den gewünschten Effekt erzielt. 

![alt text](IMG_1158.PNG)
Bild 7: Ein Blick von oben auf das Sonnensystem im realistischen Modus. Die linke Hand steuert die Geschwindigkeit der Planeten und die gelben Umlaufbahnen sind sichtbar sowie die Planeten als Kugeln. 

## 4. Verwendung von AI

ChatGPT wurde verwendet, siehe KIProtokoll.pdf für Informationen, welches mit dem Prompt "Erstelle eine professionelle Zusammenfassung dieses Chatverlaufs aus PDF, das ich dann meinem Projekt beilegen werde. Sie soll die Verwendung von KI transparent belegen und dokumentieren.".
