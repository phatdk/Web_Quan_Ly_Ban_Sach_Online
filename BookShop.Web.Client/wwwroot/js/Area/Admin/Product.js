//// view index
//function loadUserList() {
//    $.ajax({
//        url: '/User/GetAllUsers',
//        type: 'GET',
//        success: function (data) {
//            var userList = $("#prod-list");
//            userList.empty();
//            data.forEach(function (user) {
//                userList.append('<li>' + user.name + ' - Active: ' + user.status +
//                    ' <button onclick="editUser(\'' + user.id + '\')">Edit</button>' +
//                    ' <button onclick="deleteUser(\'' + user.id + '\')">Delete</button></li>');
//            });
//        },
//        error: function (error) {
//            alert('Error: ' + error.responseText);
//        }
//    });
//}
//// create
////document.getElementById('prod-type').addEventListener('change', function () {
////    var check = this.checked;  // Sử dụng this để truy cập đối tượng hiện tại
////    var booklist = document.getElementById('selectedBook');

////    if (check) {
////        booklist.setAttribute('multiple', 'multiple');
////    } else {
////        booklist.removeAttribute('multiple');
////    }
////});
//function selectedBookList() {
//    const selectElement = document.getElementById('selectedBook');
//    const selectedOptions = [];

//    // Lặp qua từng mục đã chọn và lấy giá trị của chúng
//    for (let i = 0; i < selectElement.options.length; i++) {
//        if (selectElement.options[i].selected) {
//            selectedOptions.push(selectElement.options[i].value);
//        }
//    }
//    selectedOptionsArray.forEach(function (item) {
//        // console.log(item);
//    });

//}

