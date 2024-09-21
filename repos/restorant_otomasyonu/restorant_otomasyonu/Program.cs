namespace restorant_otomasyonu
{
    using System;
    using System.Collections.Generic;

    class Program
    {
        static void Main()
        {
            Restoran restoran = new Restoran();
            restoran.AnaMenu();
        }
    }

    class Restoran
    {
        private List<Masa> masalar = new List<Masa>();
        private Dictionary<string, decimal> menu = new Dictionary<string, decimal>
    {
        { "Pizza", 50.0m },
        { "Pasta", 40.0m },
        { "Salata", 20.0m },
        { "Tatlı", 15.0m }
    };

        public Restoran()
        {
            for (int i = 0; i < 5; i++)
            {
                masalar.Add(new Masa(i + 1)); // 5 masa oluştur
            }
        }

        public void AnaMenu()
        {
            while (true)
            {
                Console.WriteLine("\nANAMENU");
                Console.WriteLine("1 - Sipariş Al");
                Console.WriteLine("2 - Hesap Al");
                Console.WriteLine("3 - Menü Düzenle");
                Console.WriteLine("4 - Çıkış");
                Console.Write("Seçiminizi yapın: ");
                string secim = Console.ReadLine();

                switch (secim)
                {
                    case "1":
                        SiparisAl();
                        break;
                    case "2":
                        HesapAl();
                        break;
                    case "3":
                        MenuDüzenle();
                        break;
                    case "4":
                        Console.WriteLine("Çıkılıyor...");
                        return;
                    default:
                        Console.WriteLine("Geçersiz seçim. Lütfen tekrar deneyin.");
                        break;
                }
            }
        }

        private void SiparisAl()
        {
            Masa masa = BosMasaBul();
            if (masa == null)
            {
                Console.WriteLine("Tüm masalar dolu.");
                return;
            }

            Console.Write("Kaç kişisiniz? ");
            int misafirSayısı = int.Parse(Console.ReadLine());
            List<List<string>> siparisler = new List<List<string>>();

            for (int i = 0; i < misafirSayısı; i++)
            {
                Console.WriteLine($"\nMüşteri {i + 1} için menü:");
                List<string> siparis = MisafirSiparişiAl();
                siparisler.Add(siparis);
                Console.Write("Başka bir arzunuz var mı? (Evet/Hayır): ");
                string başka = Console.ReadLine().ToLower();

                while (başka == "evet")
                {
                    siparis.AddRange(MisafirSiparişiAl());
                    Console.Write("Başka bir arzunuz var mı? (Evet/Hayır): ");
                    başka = Console.ReadLine().ToLower();
                }
            }

            masa.SiparisleriKaydet(siparisler); // Siparişleri masaya kaydet
            Console.WriteLine($"Masada {misafirSayısı} kişi için sipariş alındı.");
        }

        private List<string> MisafirSiparişiAl()
        {
            Console.WriteLine("Menü:");
            foreach (var item in menu)
            {
                Console.WriteLine($"- {item.Key} : {item.Value} TL");
            }
            Console.Write("Seçiminizi yapın: ");
            string siparis = Console.ReadLine();
            return new List<string> { siparis };
        }

        private Masa BosMasaBul()
        {
            foreach (var masa in masalar)
            {
                if (!masa.Dolu) // Eğer masa boşsa
                    return masa;
            }
            return null; // Tüm masalar dolu
        }

        private void HesapAl()
        {
            Console.Write("Hangi masa için hesap alacaksınız? (1-5): ");
            if (int.TryParse(Console.ReadLine(), out int masaNumarası) && masaNumarası >= 1 && masaNumarası <= 5)
            {
                masaNumarası--; // Kullanıcının girdiği değeri 0 tabanlı hale getirme

                Masa masa = masalar[masaNumarası];

                if (masa.Siparisler.Count == 0)
                {
                    Console.WriteLine("Bu masa boş veya geçersiz.");
                    return;
                }

                decimal toplam = 0;
                foreach (var siparis in masa.Siparisler)
                {
                    foreach (var item in siparis)
                    {
                        if (menu.TryGetValue(item, out decimal fiyat))
                        {
                            toplam += fiyat;
                        }
                        else
                        {
                            Console.WriteLine($"{item} menüde yok.");
                        }
                    }
                }

                Console.WriteLine($"Masa {masaNumarası + 1} için toplam hesap: {toplam} TL");
                // Hesabı ödedikten sonra masayı boşaltabilirsiniz:
                masa.Bosalt(); // Masayı boşalt
            }
            else
            {
                Console.WriteLine("Geçersiz masa numarası.");
            }
        }

        private void MenuDüzenle()
        {
            Console.Write("Menüye ne eklemek istersiniz? ");
            string yeniUrun = Console.ReadLine();

            Console.Write("Eklenecek ürünün fiyatını girin: ");
            if (decimal.TryParse(Console.ReadLine(), out decimal yeniFiyat))
            {
                menu[yeniUrun] = yeniFiyat;
                Console.WriteLine($"{yeniUrun} menüye eklendi.");
            }
            else
            {
                Console.WriteLine("Geçersiz fiyat. Menüye eklenemedi.");
            }
        }
    }

    class Masa
    {
        public int Numara { get; private set; }
        public List<List<string>> Siparisler { get; private set; }
        public bool Dolu => Siparisler.Count > 0;

        public Masa(int numara)
        {
            Numara = numara;
            Siparisler = new List<List<string>>();
        }

        public void SiparisleriKaydet(List<List<string>> siparisler)
        {
            Siparisler = siparisler;
        }

        public void Bosalt()
        {
            Siparisler.Clear();
        }
    }

}
