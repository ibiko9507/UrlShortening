Temel Uri kısaltma uygulamasıdır.
Fonksiyonalite olarak;
 - random Uri kısaltma
 - Kısaltılmış uri' ın uzun halini db'den getirme.
 - Manuel kısa uri girişi özelliklerine sahiptir.

Projenin appsettings.json dosyasından anlaşılacağı üzere Redis database kullanılmaktadır.
Db sunucu ve port bilgileri : "127.0.0.1:6379" 

Kullanılan Teknolojiler:
 .Net Core 6.0
 (Redis - Docker)

Yardımcı Kütüphaneler:
 StackExchange.Redis
 FluentValidation
----------------------------------------------------------------------------------------------------------------------------------

The basic URL shortening application. 

Functionalities:

 - Random URL shortening
 - Retrieving the long URL from the database for a shortened URL
 - Manual input of custom short URLs

The project uses Redis database, as specified in the appsettings.json file, with server and port information as "127.0.0.1:6379".

Technologies Used: 
 .Net Core 6.0 
 (Redis - Docker)

Additional Libraries: 
 StackExchange.Redis, 
 FluentValidation
