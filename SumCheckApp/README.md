# SumCheckApp - Aplicație WinForms pentru Verificare Hash Fișiere

## Descriere
Aceasta este o aplicație Windows Forms dezvoltată în C# folosind .NET 8.0 care permite utilizatorilor să calculeze și să verifice hash-ul (sumcheck) al fișierelor.

## Funcționalități
- **Selectare fișier**: Poți selecta orice fișier din sistem pentru a-i calcula hash-ul
- **Algoritmi de hash suportați**:
  - MD5
  - SHA1
  - SHA256 (default)
  - SHA384
  - SHA512
- **Calcul hash**: Calculează hash-ul fișierului selectat folosind algoritmul ales
- **Verificare integritate**: Compară hash-ul calculat cu un hash așteptat pentru a verifica dacă fișierul este integru sau a fost modificat
- **Interfață intuitivă**: Interfață grafică simplă și ușor de utilizat

## Cum se utilizează

### Cerințe
- .NET 8.0 SDK instalat
- Sistem de operare Windows (pentru WinForms)

### Construirea proiectului
```bash
cd SumCheckApp
dotnet build
```

### Rularea aplicației
```bash
dotnet run
```

### Pași de utilizare:
1. Click pe butonul **"Caută..."** pentru a selecta fișierul dorit
2. Alege algoritmul de hash din lista dropdown (implicit SHA256)
3. Click pe **"Calculează Hash"** pentru a genera hash-ul fișierului
4. (Opțional) Introdu hash-ul așteptat în câmpul corespunzător
5. Click pe **"Verifică Potrivire"** pentru a compara hash-urile
6. Rezultatul va fi afișat sub formă de mesaj și text colorat:
   - **Verde**: Hash-urile coincid - fișierul este integru
   - **Roșu**: Hash-urile nu coincid - fișierul poate fi corupt sau modificat

## Structura Proiectului
```
SumCheckApp/
├── SumCheckApp.csproj    # Fișierul de proiect .NET 8.0
├── Program.cs            # Punctul de intrare al aplicației
├── MainForm.cs           # Formularul principal cu interfața și logica
└── README.md             # Acest fișier
```

## Exemple de Utilizare

### Verificare descărcare fișier
După ce descarci un fișier de pe internet, producătorul oferă adesea un hash SHA256. Poți folosi această aplicație pentru a verifica dacă fișierul descărcat este identic cu cel original.

### Detectare modificări neautorizate
Poți calcula hash-ul unui fișier important și îl poți salva. Periodic, poți recalcula hash-ul și îl poți compara cu cel salvat pentru a detecta eventuale modificări.

## Note Tehnice
- Aplicația folosește clasele din `System.Security.Cryptography` pentru calculul hash-urilor
- Fișierele sunt citite în mod read-only pentru a preveni modificarea accidentală
- Interfața este creată programatic, fără designer Visual Studio

## Licență
Acest proiect este creat ca exemplu demonstrativ pentru învățare.
