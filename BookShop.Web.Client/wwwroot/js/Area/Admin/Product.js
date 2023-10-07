// view index
function loadUserList() {
    $.ajax({
        url: '/User/GetAllUsers',
        type: 'GET',
        success: function (data) {
            var userList = $("#prod-list");
            userList.empty();
            data.forEach(function (user) {
                userList.append('<li>' + user.name + ' - Active: ' + user.status +
                    ' <button onclick="editUser(\'' + user.id + '\')">Edit</button>' +
                    ' <button onclick="deleteUser(\'' + user.id + '\')">Delete</button></li>');
            });
        },
        error: function (error) {
            alert('Error: ' + error.responseText);
        }
    });
}
// create
//document.getElementById('prod-type').addEventListener('change', function () {
//    var check = this.checked;  // Sử dụng this để truy cập đối tượng hiện tại
//    var booklist = document.getElementById('selectedBook');

//    if (check) {
//        booklist.setAttribute('multiple', 'multiple');
//    } else {
//        booklist.removeAttribute('multiple');
//    }
//});
function selectedBookList() {
    const selectElement = document.getElementById('selectedBook');
    const selectedOptions = [];

    // Lặp qua từng mục đã chọn và lấy giá trị của chúng
    for (let i = 0; i < selectElement.options.length; i++) {
        if (selectElement.options[i].selected) {
            selectedOptions.push(selectElement.options[i].value);
        }
    }
    selectedOptionsArray.forEach(function (item) {
        // console.log(item);
    });

}

const uploadInput = document.getElementById('img-input');
const uploadedImage = document.getElementById('img-selected');
uploadInput.addEventListener('change', function (event) {

    const file = event.target.files[0];
    if (file) {
        const imageURL = URL.createObjectURL(file);
        uploadedImage.src = imageURL;
        // console.log(file)
    }
});

document.getElementById('prod-save').addEventListener('click', function createOrUpdateProduct() {
    var id = $("#prod-id").val()
    var type = $("#prod-type").prop("checked") ? 2 : 1
    var name = $("#prod-name").val()
    var quantity = $("#prod-number").val()
    var price = $("#prod-price").val()
    var description = $("#prod-description").val()
    var status = $("#prod-status").prop("checked") ? 1 : 0


    var ima = document.getElementById('img-input').files[0];
    const formData = new FormData();
    formData.append('file', ima);

    const selectElement = document.getElementById('selectedBook');
    const selectedOptions = [];

    for (let i = 0; i < selectElement.options.length; i++) {
        if (selectElement.options[i].selected) {
            selectedOptions.push(selectElement.options[i].value);
        }
    }
    const selectedOptionsArray = selectedOptions.map(item => parseInt(item, 10));
    selectedOptionsArray.forEach(function (item) {
        console.log(item);
    });
    console.log(formData)
    console.log(id)
    console.log(name)
    console.log(type)
    console.log(quantity)
    console.log(price)
    console.log(description)
    console.log(status)
    debugger
    $.ajax({
        url: id ? '/Admin/Product/Edit' : '/Admin/Product/Create',
        type: 'POST',
        data: {
            id: id, name: name, quantity: quantity, price: price, description: description, status: status, type: type,
            selectedOptions: selectedOptions,
            formData: formData
        },
        success: function () {
            alert('Product' + (id ? 'updated' : 'created') + ' successfully.');
        },
        error: function (error) {
            alert('Error: ' + error.responseText);
        }
    });
})

// delete
