using ConsoleTableExt;
using UcakBiletRezervasyon;
using UcakBiletRezervasyon.Model;

Console.WriteLine("Hoşgeldiniz...");
Console.WriteLine("Başlatılıyor...");
string basePath = AppDomain.CurrentDomain.BaseDirectory;

basePath = basePath.Replace(@"bin\Debug\net7.0\", @"database\");

string lokasyonJsonPath = basePath + "lokasyon.json";
string rezervasyonJsonPath = basePath + "rezervasyon.json";
string ucakJsonPath = basePath + "ucak.json";
string ucusJsonPath = basePath + "ucus.json";

JsonDataService<LokasyonEntity> lokasyonService = new JsonDataService<LokasyonEntity>(lokasyonJsonPath);
JsonDataService<RezervasyonEntity> rezervasyonService = new JsonDataService<RezervasyonEntity>(rezervasyonJsonPath);
JsonDataService<UcakEntity> ucakService = new JsonDataService<UcakEntity>(ucakJsonPath);
JsonDataService<UcusEntity> ucusService = new JsonDataService<UcusEntity>(ucusJsonPath);

ilkSorgu:
Console.WriteLine("");
Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
Console.WriteLine("1 - Lokasyon, Rezervasyon, Uçak veya Uçuş Listesi");
Console.WriteLine("2 - Yeni Lokasyon, Rezervasyon, Uçak veya Uçuş Kaydı");
Console.Write("Yapmak istediğiniz işlemi seçiniz : ");
string sonuc1 = Console.ReadLine();
Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");

List<LokasyonEntity>? lokasyonList = lokasyonService.ReadData();

List<RezervasyonEntity>? rezervasyonList = rezervasyonService.ReadData();

List<UcakEntity>? ucakList = ucakService.ReadData();

List<UcusEntity>? ucusList = ucusService.ReadData();

if (sonuc1 == "1")
{
    if (lokasyonList != null)
    {
        Console.WriteLine("LOKASYON LİSTESİ : ");
        ConsoleTableBuilder.From(lokasyonList).ExportAndWriteLine();
    }
    
    
    if (rezervasyonList != null)
    {
        Console.WriteLine("REZERVASYON LİSTESİ : ");
        ConsoleTableBuilder.From(rezervasyonList).ExportAndWriteLine();
    }
    
    if (ucusList != null)
    {
        Console.WriteLine("UÇUŞ LİSTESİ : ");
        ConsoleTableBuilder.From(ucusList).ExportAndWriteLine();
    }
    
    if (ucakList != null)
    {
        Console.WriteLine("UÇAK LİSTESİ : ");
        ConsoleTableBuilder.From(ucakList).ExportAndWriteLine();
    }
}

else
{
    Console.WriteLine("1 - Lokasyon");
    Console.WriteLine("2 - Rezervasyon");
    Console.WriteLine("3 - Uçak");
    Console.WriteLine("4 - Uçuş");
    Console.Write("Yeni Veri eklemek istediğiniz alanı seçiniz : ");

    int yeniEkle = Convert.ToInt32(Console.ReadLine());
    Console.WriteLine("-----------------");
    switch (yeniEkle)
    {
        case 1:
            Console.WriteLine("LOKASYON ->");
            LokasyonEntity lokasyon = new LokasyonEntity();
            Console.Write("Ülkesi : ");
            lokasyon.Ulke = Console.ReadLine();
            Console.Write("Şehri : ");
            lokasyon.Sehir = Console.ReadLine();
            Console.Write("Havaalanı : ");
            lokasyon.Havaalani = Console.ReadLine();
            lokasyon.AktifMi = true;
            lokasyon.Id = lokasyonList == null ? 1 : lokasyonList.Count == 0 ? 1 : lokasyonList.Max(p => p.Id) + 1;

            lokasyonService.WriteData(lokasyon);
            break;
        case 2:
            Console.WriteLine("REZERVASYON ->");
            
            RezervasyonEntity rezervasyon = new RezervasyonEntity();
            Console.Write("Ad : ");
            rezervasyon.Ad = Console.ReadLine();
            Console.Write("Soyad : ");
            rezervasyon.Soyad = Console.ReadLine();
            Console.Write("Yaş : ");
            rezervasyon.Yas = Convert.ToInt32(Console.ReadLine()); 
            Console.Write("Uçuş (Id) : ");
            rezervasyon.UcusId = Convert.ToInt32(Console.ReadLine());
            rezervasyon.Id = rezervasyonList == null ? 1 : rezervasyonList.Count == 0 ? 1 : rezervasyonList.Max(p => p.Id) + 1;
            
            //Uçuş var mı kontrolü
            UcusEntity? ucusBilgisi = ucusList?.FirstOrDefault(c => c.Id == rezervasyon.UcusId);

            if (ucusBilgisi == null)
            {
                Console.WriteLine("Uçuş Bilgisi Bulunamadı!!!");
                break;
            }
            
            //Uçak var mı kontrolü
            UcakEntity ucakBilgisi = ucakList?.FirstOrDefault(c => c.Id == ucusBilgisi.UcakId);

            if (ucakBilgisi == null)
            {
                Console.WriteLine("Uçak Bilgisi Bulunamadı!!!");
                break;
            }

            rezervasyonService.WriteData(rezervasyon);
            break;
        
        case 3:
            Console.WriteLine("UÇAK ->");
            UcakEntity ucak = new UcakEntity();
            Console.Write("Marka : ");
            ucak.Marka = Console.ReadLine();
            Console.Write("Model : ");
            ucak.Model = Console.ReadLine();
            Console.Write("Seri no : ");
            ucak.SeriNo = Console.ReadLine();
            Console.Write("Koltuk sayısı : ");
            ucak.KoltukSayisi = Convert.ToInt32(Console.ReadLine());
            ucak.Id = ucakList == null ? 1 : ucakList.Count == 0 ? 1 : ucakList.Max(p => p.Id) + 1;
            
            ucakService.WriteData(ucak);
            break;
        
        case 4:
            Console.WriteLine("UÇUŞ ->");
            UcusEntity ucus = new UcusEntity();
            Console.Write("Saati : ");
            ucus.Saat = Console.ReadLine();
            Console.Write("Lokasyon (Id) : ");
            ucus.LokasyonId = Convert.ToInt32(Console.ReadLine());
            Console.Write("Uçak (Id) : ");
            ucus.UcakId = Convert.ToInt32(Console.ReadLine());
            ucus.Id = ucusList == null ? 1 : ucusList.Count == 0 ? 1 : ucusList.Max(p => p.Id) + 1;
            
            //Lokasyon var mı kontrolü
            LokasyonEntity lokasyonKontrol = lokasyonList?.FirstOrDefault(c => c.Id == ucus.LokasyonId);

            if (lokasyonKontrol == null)
            {
                Console.WriteLine("Lokasyon Bilgisi Bulunamadı!!!");
                break;
            }
            
            //Uçak var mı kontrolü
            UcakEntity ucakBilgisi2 = ucakList?.FirstOrDefault(c => c.Id == ucus.UcakId);

            if (ucakBilgisi2 == null)
            {
                Console.WriteLine("Uçak Bilgisi Bulunamadı!!!");
                break;
            }

                ucusService.WriteData(ucus);
            break;
        
        default:
            Console.WriteLine("Geçersiz numara seçimi!!!");
            break;
    }
}

goto ilkSorgu;
