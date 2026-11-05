# 🐍 C# Windows Forms Snake Game (SQL Server & ADO.NET Entegreli)

![C#](https://img.shields.io/badge/C%23-.NET%208.0-blue)
![Windows Forms](https://img.shields.io/badge/UI-Windows%20Forms-brightgreen)
![SQL Server](https://img.shields.io/badge/Database-SQL%20Server-red)
![ADO.NET](https://img.shields.io/badge/Data-ADO.NET-orange)

Klasik Yılan Oyunu mekaniklerinin modern C# WinForms ve **ADO.NET** veritabanı altyapısıyla harmanlandıldığı, dinamik bonus yem sistemine ve SQL Server tabanlı liderlik tablosuna sahip masaüstü projesi.

---

## 📸 Ekran Görüntüleri (Screenshots)

| Oyun İçi Görüntü | Liderlik Tablosu & Skor Kaydı |
| :---: | :---: |
| ![Oyun Görüntüsü]---<img width="747" height="752" alt="Oyun Başlangıcı" src="https://github.com/user-attachments/assets/86198521-3b22-49b1-a733-dd85341d1dbb" /> 

| ![Liderlik Tablosu] |
<img width="750" height="752" alt="Skor Tablosu" src="https://github.com/user-attachments/assets/2dbaddba-9ff5-43f9-b2b3-8e177967fedd" />





## 🌟 Öne Çıkan Özellikler

* **🎮 Klasik Yılan Mekanikleri:** Esnek ve dinamik gövde takibi, klavye ok tuşlarıyla akıcı kontrol.
* **🌐 Kesintisiz Oyun Alanı (Teleportation):** Yılan harita sınırlarından çıktığında karşı duvardan ekrana geri döner.
* **⭐ Dinamik Özel Yem Mekanizması:** Her 10 normal yem yendiğinde ortaya çıkan, +2 puan kazandıran ve Altın Sarısı renkte beliren süreli/özel yem sistemi.
* **🗄️ SQL Server & ADO.NET Entegrasyonu:** Oyuncunun skoru bittiğinde ismiyle birlikte `DbSnakeGame` veritabanındaki `Tbl_Scores` tablosuna kaydedilir.
* **🏆 Canlı Liderlik Tablosu:** SQL sorgusu (`SELECT TOP 5`) ile veritabanından çekilen en yüksek skorlar `DataGridView` üzerinde kaydırma çubuğu olmadan temiz bir şekilde listelenir.
* **📸 Snapshot (Ekran Görüntüsü Kaydı):** Oyun sonundaki skoru ve yüksek skoru filigranlı bir etiketle `.png` / `.jpg` formatında bilgisayara kaydetme özelliği.

---

## 🛠️ Kullanılan Teknolojiler

* **Dil:** C# (.NET Core / Modern .NET Framework)
* **Arayüz:** Windows Forms (WinForms)
* **Veritabanı:** Microsoft SQL Server
* **Veri Erişimi:** ADO.NET (`SqlConnection`, `SqlCommand`, `SqlDataAdapter`)
* **Çizim Motoru:** GDI+ (`System.Drawing`, `PaintEventArgs`)

---

## 🚀 Projeyi Yerel Kurulumla Çalıştırma

### 1. SQL Server Veritabanı Kurulumu
SQL Server Management Studio (SSMS) uygulamasında aşağıdaki T-SQL kodunu çalıştırarak veritabanı ve tabloyu oluşturun:

```sql
CREATE DATABASE DbSnakeGame;
GO
USE DbSnakeGame;

CREATE TABLE Tbl_Scores (
    ScoreId INT PRIMARY KEY IDENTITY(1,1),
    PlayerName NVARCHAR(50),
    Score INT,
    Date DATETIME DEFAULT GETDATE()
);
