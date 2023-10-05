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