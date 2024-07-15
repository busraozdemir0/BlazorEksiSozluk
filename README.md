# Ekşi Sözlük Sitesi
## Proje hakkında

###
Bu proje; Ekşi Sözlük sitesinin tasarımı ve çalışma mantığı göz önünde bulundurularak yapılmıştır. Giriş yapan kullanıcı kendi bilgilerini güncelleyebilir, entry oluşturabilir, var olan entry'lerin detaylarına
giderek ilgili entry'e ait yorum yapabilir ya da ilgili entry'i favorilere ekleyebilmektedir.

.Net Core 6.0 ve .Net Core 6.0 Web API kullanılarak geliştirilen sözlük sitesinde; RabbitMQ, Blazor, EF6 gibi teknolojiler ve CQRS, Onion Architecture, MediatR gibi yaklaşımlar kullanılmaktadır. 
###

# Kullanılan Teknolojiler & Yaklaşımlar
- .Net Core 6.0
- .Net Core 6.0 Web API
- Blazor Web Assembly
- MSSQL Server
- Onion Architecture
- CQRS
- Entity Framework Code First
- Dapper
- MediatR
- RabbitMQ
- Swagger
- Html, Css
- Bootstrap
- Automapper
  
# Projenin Öne Çıkan Özellikleri
- Veritabanı işlemleri için Entity Framework Code First kullanımı
- Listelenen Entry'ler içerisinde ilgili entry'nin detayına gidebilme
- İlgili entry'e yorum yapabilme, favorilere ekleme, entry'e oy verme/vermeme (Up-Down)
- Entry oluşturma
- Profilim sayfasında kullanıcı bilgilerini ya da şifresini güncelleyebilme
- Entry listesi için sayfalama yapısı
- Entry arama işlemi

# Projenin Görselleri

### Veritabanı Diyagramı 
![Ana ekran](https://github.com/busraozdemir0/BlazorEksiSozluk/blob/master/DB-ProjectScreenShots/db.png)

### Ana Sayfa
![Ana sayfa](https://github.com/busraozdemir0/BlazorEksiSozluk/blob/master/DB-ProjectScreenShots/home.png)

### Giriş Yaptıktan Sonra
![Ana sayfa](https://github.com/busraozdemir0/BlazorEksiSozluk/blob/master/DB-ProjectScreenShots/loginAfter.png)

### Entry Oluşturma Sayfası
![Ana sayfa](https://github.com/busraozdemir0/BlazorEksiSozluk/blob/master/DB-ProjectScreenShots/entryCreate.png)

### Profilim Sayfası
![Ana sayfa](https://github.com/busraozdemir0/BlazorEksiSozluk/blob/master/DB-ProjectScreenShots/profile.png)

![Ana sayfa](https://github.com/busraozdemir0/BlazorEksiSozluk/blob/master/DB-ProjectScreenShots/profileUpdate.png)

### Arama İşlemi (Aranan ifade: ma)
![Ana sayfa](https://github.com/busraozdemir0/BlazorEksiSozluk/blob/master/DB-ProjectScreenShots/search.png)

### Entry Detay Sayfası
![Ana sayfa](https://github.com/busraozdemir0/BlazorEksiSozluk/blob/master/DB-ProjectScreenShots/entryDetail.png)

### Entry Detay Sayfasında İlgili Entry'e Ait Yorum Yapabilme Kısmı
![Ana sayfa](https://github.com/busraozdemir0/BlazorEksiSozluk/blob/master/DB-ProjectScreenShots/EntryCommentCreate.png)

### Sayfalama Yapısı
![Ana sayfa](https://github.com/busraozdemir0/BlazorEksiSozluk/blob/master/DB-ProjectScreenShots/paging.png)
