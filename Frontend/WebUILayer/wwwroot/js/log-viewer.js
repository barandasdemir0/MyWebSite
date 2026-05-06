document.addEventListener("DOMContentLoaded", function () {
    // Sayfadaki tüm detay butonlarını bul
    const detailButtons = document.querySelectorAll('.btn-log-detail');

    detailButtons.forEach(button => {
        button.addEventListener('click', function () {
            // Butonun üzerindeki data-exception değerini oku
            const exceptionMessage = this.getAttribute('data-exception');

            if (exceptionMessage) {
                // Tarayıcının standart mesaj kutusunu kullan (Minimum JS)
                alert(exceptionMessage);

                // İstersen buraya console.log da ekleyebilirsin:
                console.error("Log Detayı:", exceptionMessage);
            }
        });
    });
});
