function formatDate(date) {
	date = new Date(date);
	var year = date.getFullYear();
	// Lấy tháng và thêm 1 vì tháng bắt đầu từ 0
	var month = (date.getMonth() + 1).toString().padStart(2, '0');
	var day = date.getDate().toString().padStart(2, '0');
	var hours = date.getHours().toString().padStart(2, '0');
	var minutes = date.getMinutes().toString().padStart(2, '0');
	var seconds = date.getSeconds().toString().padStart(2, '0');
	// Định dạng theo yêu cầu "yyyy-MM-dd hh:mm:ss"
	var formattedDate = `${year}-${month}-${day} ${hours}:${minutes}:${seconds}`;

	return formattedDate;
}
