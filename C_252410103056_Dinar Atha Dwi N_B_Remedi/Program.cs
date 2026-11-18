using System;
using System.Collections.Generic;

namespace MafiaGalon
{
    //Class Agen
    public class Agen
    {
        public string IdAgen { get; set; }
        public string NamaAgen { get; set; }
        public int StokAgen { get; set; }

        public Agen(string idAgen, string namaAgen, int stokAwal)
        {
            IdAgen = idAgen;
            NamaAgen = namaAgen;
            StokAgen = stokAwal;
        }

        public void TampilInfo()
        {
            Console.WriteLine($"[AGEN] ID: {IdAgen} | Nama Markas: {NamaAgen} | Stok Saat Ini: {StokAgen} galon");
        }
    }

    //Enkapsulasi dan abstract
    public abstract class ProdukGalon
    {
        private string idProduk;
        private string namaProduk;
        private int stokGudang;

        public string IdProduk
        {
            get { return idProduk; }
            set { idProduk = value; }
        }

        public string NamaProduk
        {
            get { return namaProduk; }
            set { namaProduk = value; }
        }

        public int StokGudang
        {
            get { return stokGudang; }
            private set
            {
                if (value < 0)
                    stokGudang = 0;
                else
                    stokGudang = value;
            }
        }

        public ProdukGalon(string idProduk, string namaProduk, int stokAwal)
        {
            IdProduk = idProduk;
            NamaProduk = namaProduk;
            StokGudang = stokAwal;
        }

        public void KurangiStok(int jumlah)
        {
            if (jumlah > 0 && jumlah <= stokGudang)
            {
                stokGudang -= jumlah;
            }
        }

        // Polymorphism dan abstract
        public abstract decimal HitungBiaya(int jumlah);

        public virtual void TampilInfoProduk()
        {
            Console.WriteLine($"[PRODUK] ID: {IdProduk} | Nama: {NamaProduk} | Stok Gudang Pusat: {StokGudang} galon");
        }
    }

    //Inheritence
    public class GalonAkua : ProdukGalon
    {
        public GalonAkua(string idProduk, string namaProduk, int stokAwal)
            : base(idProduk, namaProduk, stokAwal)
        {
        }

        public override decimal HitungBiaya(int jumlah)
        {
            return jumlah * 20000;
        }
    }

    public class GalonBerkeran : ProdukGalon
    {
        public string JenisKeran { get; set; }

        public GalonBerkeran(string idProduk, string namaProduk, int stokAwal, string jenisKeran)
            : base(idProduk, namaProduk, stokAwal)
        {
            JenisKeran = jenisKeran;
        }

        public override decimal HitungBiaya(int jumlah)
        {
            return jumlah * 12000;
        }

        public override void TampilInfoProduk()
        {
            Console.WriteLine($"[PRODUK] ID: {IdProduk} | Nama: {NamaProduk} (Keran: {JenisKeran}) | Stok Gudang Pusat: {StokGudang} galon");
        }
    }

    //Interface
    public interface IDapatDidistribusikan 
    {
        void KirimPaket();
    }

    public class DistribusiGalon : IDapatDidistribusikan
    {
        public Agen AgenTujuan { get; set; }
        public ProdukGalon ProdukDikirim { get; set; }
        public int Jumlah { get; set; }

        public DistribusiGalon(Agen agenTujuan, ProdukGalon produkDikirim, int jumlah)
        {
            AgenTujuan = agenTujuan;
            ProdukDikirim = produkDikirim;
            Jumlah = jumlah;
        }

        public void KirimPaket()
        {
            Console.WriteLine($"\n Memproses Pengiriman {Jumlah} galon {ProdukDikirim.NamaProduk} ke {AgenTujuan.NamaAgen} ");

            if (Jumlah <= 0)
            {
                Console.WriteLine("[GAGAL] Operasi dibatalkan! Jumlah galon yang dikirim harus lebih dari 0.");
                return;
            }

            if (Jumlah > ProdukDikirim.StokGudang)
            {
                Console.WriteLine($"[GAGAL] Pasokan tidak cukup! Gudang hanya memiliki {ProdukDikirim.StokGudang} galon, tidak bisa mengirim {Jumlah} galon.");
                return;
            }

            ProdukDikirim.KurangiStok(Jumlah); 
            AgenTujuan.StokAgen += Jumlah;   
            decimal totalBiaya = ProdukDikirim.HitungBiaya(Jumlah);

            Console.WriteLine($"[SUKSES] Paket rahasia ({Jumlah}x {ProdukDikirim.NamaProduk}) berhasil dikirimkan ke agen '{AgenTujuan.NamaAgen}'!");
            Console.WriteLine($"         Total Setoran Kartel: Rp{totalBiaya:N0}");
        }
    }

    public class SindikatGalon
    {
        private List<Agen> daftarAgen = new List<Agen>();
        private List<ProdukGalon> daftarProduk = new List<ProdukGalon>();

        public void TambahAgen(Agen agen)
        {
            daftarAgen.Add(agen);
        }

        public void TambahProduk(ProdukGalon produk)
        {
            daftarProduk.Add(produk);
        }

        public void TampilkanSindikat()
        {
            Console.WriteLine("LAPORAN ASET & PASUKAN SINDIKAT MAFIA GALON");
            Console.WriteLine(">> Daftar Agen :");
            foreach (var agen in daftarAgen)
            {
                agen.TampilInfo();
            }

            Console.WriteLine("\n>> Stok Gudang :");
            foreach (var produk in daftarProduk)
            {
                produk.TampilInfoProduk();
            }
        }
    }

    //Program Utama
    class Program
    {
        static void Main(string[] args)
        {
            Agen agenUtara = new Agen("AG-01", "Markas Sektor Utara", 10);
            Agen agenSelatan = new Agen("AG-02", "Bunker Pelabuhan Selatan", 5);
            ProdukGalon produk1 = new GalonAkua("PRD-01", "Air Galon Akua Murni", 50);
            ProdukGalon produk2 = new GalonBerkeran("PRD-02", "Air Galon Berkeran Impor", 30, "Keran Putar Stainless");
            SindikatGalon kartelMetropolis = new SindikatGalon();
            kartelMetropolis.TambahAgen(agenUtara);
            kartelMetropolis.TambahAgen(agenSelatan);
            kartelMetropolis.TambahProduk(produk1);
            kartelMetropolis.TambahProduk(produk2);

            kartelMetropolis.TampilkanSindikat();

            Console.WriteLine("\n Simulasi Perhitungan Biaya ");
            Console.WriteLine($"Harga 5 {produk1.NamaProduk}: Rp{produk1.HitungBiaya(5):N0}");
            Console.WriteLine($"Harga 5 {produk2.NamaProduk}: Rp{produk2.HitungBiaya(5):N0}");

            DistribusiGalon kirim1 = new DistribusiGalon(agenUtara, produk1, 15);
            kirim1.KirimPaket();

            DistribusiGalon kirim2 = new DistribusiGalon(agenSelatan, produk2, 100);
            kirim2.KirimPaket();

            DistribusiGalon kirim3 = new DistribusiGalon(agenSelatan, produk2, -5);
            kirim3.KirimPaket();

            kartelMetropolis.TampilkanSindikat();

            Console.ReadLine();
        }
    }
}