# RabbitMQ-With-NETCore

### Message Queue Nedir :
+ Yazılım sistemlerinde iletişim için kullanılan yapılardır.
+ Bir birinden bağımsız iki yazılım arasında ver alışverişi yapmak.
+ Message Queue gönderilen mesajları kuyrukta saklar ve sonradan bu mesajları
  işlenmesini sağlar
+ Kuyruğa mesaj gönderene Producer(Yayıncı) ya da publisher, kuyruktan mesaj
  işleyene ise Consumer(Tüketici) denir.
+ Message burda veriye denk geliyor.

Not: Operasyonları farklı servisler üzerinden işlemek lazım.
     Eticaret ama faturalama sistemi başka bir serviste.

Not: Messajlar sırasıyla işlenir.

### Senkron - Asenkron
+ Sitemler bir biriyle haberleşirken cevep bekliyorlarsa bu senkrondur.
+ Sistemler bir biriyle haberleşirken cevap beklemiyor bu asenkron.

### Message Broker Nedir:
+ İçerisinde message queue yu barındıran ve bu queue üzerinden publisher/producer ile
  consumer arasındaki ileitşimi sağlayan sistemdir.

### RabbitMQ yu Neden Kullanmalıyız
+ Yazılım uygulamalarında ölçeklendirilebilir ortam sağlamak için.
+ Uygulamarda kullanıcılardan gelen işlemleri anlık cevap veremiyorsak kullanıcıyı oyalamadan asenkron yapıda çalışmamız gerekir. Asenkron yapılanmayı kontrol edicek yapı RabbitMQ'dur.
+ Response time'ı uzun sürebilecek operasyonları uygulamadan bağımsızlaştırarak burdaki sorumluluğu farklı ir uygulamanın üstlenmesini sağlayacak olan bir mekanizma sunmaktadır. Ana uygulamdaki yoğunluk azalacaktır.

Not: RabbitMQ bir message broker olduğu için mesajları yayınlayan producer ve bu mesajları tükene consumer servisleri taragından olarak kullanılmaktadır.

Not: Yapısal olarak exchange ve queue ensturmanları üzerinden işlevsellik göstermektedir.

Not: Burada producer ve consumer 'ın hangi platformda geliştirildiğinin hiçbir önemi yoktur. Mimari yazılım dilinden bağımsız işlev görür.

Not: Bu süreçte RabbitMQ, AMQP protokolünü kullanarak faliyet gösterir.

### Exchange Nedir:
+ Mesajların nasıl işleneceğinin modelini sunar.
+ Publisher tarafından gönderilen mesajların nasıl yönetileceğini ve hangi route'lara yönlendireleceğini belirlememiz konusunda kontrol sağlayan yapıdır.

### Route Nedir:
+ Route ise mesajların exchange üzerinden kuyruklara nasıl gönderileceğini tanımlayan mekanizmadır.
+ Bu süreçte exchangede bulunan routing key değeri kullanılır.

### Binding Nedir:
+ Exchange ile Queue lar arası bağlanmaya binding denir.

### Dricet Exchange:
+ Mesajları direkt olarak belli bir kuyruğa göndermeyi sağlayan exchange'dir

### Fanout Exchange
+ Mesajların, bu exchange'e bind olmuş olan tüm kuyruklara gönderilmesini sağlar.

### Topic Exchange
+ Routing Key'leri kullanarak mesajları kuyruklara yönlendirmek için kullanılan bir exchange'dir. Bu exchange ile routing key'in bir kısmına keylere göre kuyruklara mesaj gönderilir.

### Header Exchange
+ Routing key yerine header'ları kullanarak mesajları kuyruklara yönlendirmek için kullanılan exchange'dir.

### RabbitMQ yu NETCore Üzerinde Çalıştırmak
+ RabbitMQ.Client kütüphanesinin projeye yüklenmesi gerekir.

### Publisher Uygulaması İşlem Sırası
+ Bağlantı Oluşturma.
+ Bağlantıyı Aktifleştirme ve Kanal Açma
+ Queue Oluşturma
+ Queue'ye Mesaj Gönderme

### Consumer Uygulaması İşlem Sırası
+ Bağlantı Oluşturma.
+ Bağlantıyı Aktifleştirme ve Kanal Açma
+ Queue Oluşturma
+ Queue'dan Mesaj Alma

### Gelişmiş Kuyruk Mimarisi Nedir:
+ RabbitMQ teknolojisinin ana fikri, yoğun kaynak gerektiren işleri hemen yapmaya koyularak tamamlanmasını beklemek zorunda kalmaksızın bu işleri ölçeklendirilebilir bir vaziyette daha sonra yapılacak şekilde planlamaktadır.
+ Tüm bu süreçte kuyrukların bakımı gerekmektedir. Mesajların kalıcılığına dair durumlar cs. konfigure edilmesi gerekmektedir. Ayrıca birden fazla tüketicinin söz konusu olduğu durumlarda nasıl davranışın olacağı vs. durumları da oldukça önem arz etmektedir.
+ Gelişmiş kuyruk mimarisi işte burda devreye girmektedir. Kuyrukların ve mesajların kalıcılığı, mesajların birden fazla tüketiciye karşı dağıtım stratejisi yahut tüketici tarafından işlenmiş bir mesajın kuyruktan silinebilmesi için onay sistemi vs. tüm bu detaylar bu başlığa girmektedir.

### Round-Robin Dispatching:
+ RabbitMq, defaut olarak tüm consumerlara sırasıyla mesaj gönderir.

### Message Acknowledgement
+ RabbitMQ, tüketiceye gönderdiği mesajı başarılı bir şekilde işlensin veya işlenmesin hemen kuyruktan silinmesi üzere işaretlenmiştir.
+ Tüketicilerin ilgili mesajın işlenemediği zaman tüketicen rabbitMQ'nun bildirilmesi gerekmektedir. 

### Message Acknowledgement Problemleri Nelerdir:
+ Message işlenmenden consumer tarafından problem yaşarsa bu mesajın sağlıklı bir şekilde işlenebilmesi için başka bir consumer tarafından tüketilebilir olmalıdır.
+ Aksi taktirde mesaj kuyruktan tüketici tarafından alındığı an silinirse bu durumda veri kaybı ihtimali söz konusu olacaktır. İşte bu tarz durumlar için Message Acknowlegement özelliği şarttır.
+ Tabii RabbitMQ ya mesajın silinmesi için haber göndermeyi unutmamak gerek. Yoksa veri silinmez ve tutarsızlık olur.

### Message Acknowledgement Nasıl Yapılandırılır:
+ RabbitMQ'da mesaj onaylama sürecini aktifleştirebilmek için consumer uygulamasında 'BasicConsume' metodundaki 'aoutoAck' parametresini false değerine getirebiliriz. Böylece otomatik silinmeyecek consumerdan onay bekleyecektir.
+ Consumer mesajın başarıyla işlendiğine dair uyarıyı 'channel.Basic.Ack' metodu ile gerçekleştirir.

Not: multiple parametresi birden fazla mesaja dair onay bildirisi gönderir .Eğer true değeri verilirse DeliveryTag değerine sahip olan bu mesajla birlikte bundan önceki mesajların da onaylandığını belirtir. False verilirse sadece ilgili mesajı onaylandığını belirtir.

### BasicNack İle İşlenmeyen Mesajları Geri Gönderme:
+ İlgili mesajın işlenmeyeceğine dair bildiride bulunmak gerekebilir.
+ Böyle bir durumda 'channel.BasicNack' metodunu kullanarak RabbitMQ'ya bilgi verebilir ve mesajı tekrardan işletebilirz.
+ Buişlemlerde requeue parametresi önem arz eder. Bu parametre, bu consumer tarafıdnan işlenemeyeceği ifade edilen bu mesajın tekrardan kuyruğa eklenip eklenmemesinin kararını vermektedir. True verildiği taktirde mesaj kuyruğa terardan işlenmek üzere eklenecek, false değerinde ise kuyruğa eklenmeyerek silinecektir.

### BasicCacnle İle Bir Kuyruktaki Tüm Mesajların İşlenmesini Reddetmek
+ Bu metod ile verilen consumerTag değerine karşılık gelen queue'daki tüm mesajlar reddedilerek, işlenmez

### BasicReject İle Tek Bir Mesajın İşlenmesini Reddetme
+ RabbitMQ'da kuyrukta bulunan mesajlardan belirli olanların consumer tarafından işlenmesini istediğimiz durumlarda BasicReject metodunu kullabilirz.

### Message Durability
+ Olası sunucu kapanmasında mesajlar silinir.
+ Böyle bir durumda kuyruk ve mesaj açısından kalıcı olarak işaratleme yapmamız gerekmektedir.
+ Gelişmiş kuyruk yapısı kodunda ilgili işlemler yapılmıştır.

### Fair Dispatch
+ Consumerlara eşit şekilde mesajların iletilmesi.

### Mesaj İşleme Konfigürasyonu
+ RabbitMQ'da BasicQos metodu ile mesajların işleme hızını ve teslimat sırasını belirleyebilirz Böylece Fair Dispatch özelliğini konfigüre edebeilirz.
+ prefetchSize : bir consumer tarafından alınabailcek en büyük mesaj bouyutunu byte cinsinden belirler. 0, sınırsız demektir.
+ prefetchCount: Bir consumer tarafından aynı anda işleme alınabilecek mesaj sayısını belirler.
+ global: Tüm consumerlar için mi yoksa sadece çağrı yapılan için mi geçerli olacağını belirtir.

### Mesaj Tasarımları
+ Design patternlar gibidir, yaspısal davranışları belirler.
+ İki servis arasında iletilecek mesajların nasıl iletileceğini, nasıl işleneceğini, ne şekilde yapılandırılıacağını ve ne tür bilgiler tanışayacağını belirler.
+ Her tasarım farklı bir uygulama senaryosuna göre şekillenmekte ve en iyi sonuçlar alınabilecek şekilde yapılandırılmaktadır.

### Yaygın Olarak Kullanılan Mesaj Tasarımları
+ P2P(Point to Point) Tasarımı = Bu tasarımda, bir publisher ile mesajı direkt bir kuyruğa gönderir ve bu mesajı kuyruğu işleyen bir consumer tarafından tüketilir. Eğer ki seneryo gereği bir mesajın bir tüketici tarafından işlenmesi gerekiyorsa bu yaklaşım kullanılır.
+ Publish/Subscrine(Pub/Sub) Tasarımı = Bu tasarımda publisher bir exchange'e gönderir ve böylece mesaj bu exchange'e bind edilmiş olan tüm kuyruklara yönlendirilir. Bu tasarım, bir mesajın birçok tüketici tarafından işlenmesi gerektiği durumlarda kullanışlıdır.
+ Work Queue(İş Kuyruğu) Tasarımı = Bu tasarımda, publisher tarafından yayınlamış bir mesajın birden fazla consumer arasından yalnızca biri tarafından tüketilmesi amaçlanmaktadır. Böylece mesajların işlenmesi sürecinde tüm consumer'lar aynı iş yüküne ve eşit görev dağılımına sahip olacaktırlar. Work Queue tasarımı, iş yükünün dağıtılmassı gereken ve paralel işleme ihtiyacı duyulan senaryolar içim oldukça uygundur.
+ Request/Response Tasarımı = Bu tasarımda, publisher bir request yapar gibi kuyruğa mesaj gönderir ve bu mesajı tüketen consumer'dan sonuca dair başka kuyruktan bir yanıt/response bekler. Bu tarz seneryolar için oldukça uygun bir tasarımdır. Özünde producer ayriyetten bir consumerdır, consumer ise ayriyetten producerdır.

### ESB(Enterprise Service Bus) Nedir
+ ESB, sevşsler arası entegrasyonu sağlayan komponentlerin bütünüdür diyebiliriz.
+ Farklı yazılım sistemlerinin birbirleriyle iletişim kurmasını sağlamak için kullanılan bir yazılım mimarisi ve araç setidir.
+ ESB, RabbitMQ gibi farklı sistemlerin birbirleriyle etkileşime girmesini sağlayan teknolojilerin kullanımını ve yönetilebilirliğini kolaylaştırmakta ve buna bir ortam sağlamaktadır.
+ ESB, serviseler arası etkileşim süreçlerinde aracı uygulamalara karşın yüksek bir abstraction görevi görmekte ve böylece bütünsel olarak sistemşn tek bir teknolojiye bağımlı olmasını engellemektedir.

### MassTransit Nedir:
+ .NET için geliştirilmiş olan distributed uygulamaları rahatlıkla yönetmeti ve çalıştırmayı amaçlayan open source bir entreprise service bus frameworküdür
+ MassTransit, tamamen farklı uygulamalar arasında Message-Based Communication yapabilmemizi sağlayan bir transport gatewaydir.

### Transport Gateway Nedir:
+ Farklı sistemler arasında farklı iletişim protokollerini kullanarak sistemler arasında iletişim kurmayı sağlayan bir araçtır.


