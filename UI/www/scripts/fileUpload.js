var picture = $("#addPicture");
var image = $("#myImage");

var data = new Object();

picture.click(function () {
    var buttons = [
   {
       text: "Yeni Fotoğraf",
       onClick: function () {
           navigator.camera.getPicture(onSuccess, onFail, {
               quality: 50,
               destinationType: Camera.DestinationType.DATA_URL,
               saveToPhotoAlbum: true,
               correctOrientation: true,
               Direction: true
           });
       }
   },
   {
       text: "Galeriden Yükle",
       onClick: function () {
           navigator.camera.getPicture(onSuccess, onFail,
            {
                quality: 50,
                destinationType: navigator.camera.DestinationType.DATA_URL,
                sourceType: navigator.camera.PictureSourceType.PHOTOLIBRARY,
                correctOrientation: true,
                Direction: true
            }
        );
       }
   },
   {
       text: "Vazgeç",
       color: 'red'
   }];
    myApp.actions(buttons);
});

function onSuccess(imageData) {
    image.attr("src", "data:image/jpeg;base64," + imageData);
    data.imageData = imageData;
}

function onFail(error) {
    alert("Resim yükleme sırasında bir hata meydana geldi.");
}

function Send() {
    if (data.imageData != null) {
        $.ajax({
            type: "POST",
            dataType: 'json',
            contentType: "application/json",
            url: 'http://photoupload.workstudyo.com/api/PhotoUpload/PhotoUpload/',
            data: JSON.stringify(data)
        }).done(function (imageUrl) {
            myApp.hidePreloader();
            myApp.alert("Resim yükleme işleminiz tamamlandı.");
        }).error(function (xhr) {
            myApp.hidePreloader();
            myApp.alert(xhr.responseText);
            myApp.alert(xhr.status);
        });
    }
    else {
        alert("Lütfen resim seçiniz.")
    }

}