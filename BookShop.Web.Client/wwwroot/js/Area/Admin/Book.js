document.addEventListener("DOMContentLoaded", function () {
    var video = document.getElementById('video');
    var result = document.getElementById('result');

    // Kiểm tra xem trình duyệt có hỗ trợ getUserMedia không
    if (navigator.mediaDevices && navigator.mediaDevices.getUserMedia) {
        navigator.mediaDevices.getUserMedia({ video: true }).then(function (stream) {
            video.srcObject = stream;
            video.play();
        });
    }

    // Đợi video sẵn sàng để bắt đầu quét
    video.addEventListener('canplay', function () {
        setTimeout(function () {
            decode();
        }, 500);
    });

    function decode() {
        try {
            result.innerText = '';
            // Sử dụng hàm ZXing để quét mã vạch từ video
            qrcode.decode();
        } catch (e) {
            setTimeout(decode, 300);
        }
    }

    // Xử lý kết quả khi quét thành công
    qrcode.callback = function (result) {
        if (result instanceof Error) {
            console.log('Quét mã vạch thất bại: ' + result);
        } else {
            console.log('Mã vạch được quét: ' + result);
            // Xử lý mã vạch ở đây (ví dụ: hiển thị trên trang web)
            document.getElementById('result').innerText = 'Barcode: ' + result;
        }
        setTimeout(decode, 1000);
    };
});

