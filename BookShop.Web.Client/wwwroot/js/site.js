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

function createPagingButtons(min, max, index) {
    var buttonContainer = document.getElementById("buttonContainer");
    buttonContainer.innerHTML = "";
    intIndex = parseInt(index);
    if (intIndex < 3) {
        limitStart = min;
        limitEnd = intIndex + 3;
        if(limitEnd > max) limitEnd = max
    } else if (intIndex > max - 2) {
        limitStart = intIndex - 3;
        limitEnd = max;
        if(limitStart < min) limitStart = min
    } else {
        limitStart = intIndex - 2;
        limitEnd = intIndex + 2;
    }

    for (var i = limitStart; i <= limitEnd; i++) {
        var button = document.createElement("button");
        if (i == limitStart) {
            button.style.margin = "5px";
            button.style.width = "50px"
            button.style.border = "1px solid black";
            button.className = "startPage";
            button.value = min;
            button.innerText = "<<";

            button.addEventListener("click", function () {
                loadData(min);
                createButtons(minpage, maxpage, page);
            });
            buttonContainer.appendChild(button);
            button = document.createElement("button");
            button.style.margin = "5px";
            button.style.width = "50px";
            button.style.border = "1px solid black";
            button.className = "previousButton";
            button.value = intIndex == 0 ? "1" : intIndex - 1;
            button.innerText = "<";

            button.addEventListener("click", function () {
                if (this.value > 0) {
                    loadData(this.value);
                } else loadData(min);
            });
            buttonContainer.appendChild(button);
        }
        button = document.createElement("button");
        button.style.minWidth = "30px";
        button.style.margin = "5px";
        button.style.border = "1px solid black";
        button.className = "pagingButton";
        button.value = i;
        button.innerText = i;
        button.addEventListener("click", function () {
            loadData(this.value);
        });

        buttonContainer.appendChild(button);
        // changeStyle;
        if (i == limitEnd) {
            button = document.createElement("button");
            button.style.width = "50px";
            button.style.margin = "5px";
            button.style.border = "1px solid black";
            button.className = "nextButton";
            button.value = intIndex + 1;
            button.innerText = ">";
            button.addEventListener("click", function () {
                if (this.value < max + 1) {
                    loadData(this.value);
                } else loadData(max);
            });
            buttonContainer.appendChild(button);

            button = document.createElement("button");
            button.style.width = "50px";
            button.style.margin = "5px";
            button.style.border = "1px solid black";
            button.className = "endPage";
            button.value = max;
            button.innerText = ">>";
            button.addEventListener("click", function () {
                loadData(max);
            });
            buttonContainer.appendChild(button);
        }
        var allButtons = document.querySelectorAll(".pagingButton");
        allButtons.forEach(function (item) {
            if (item.value == intIndex) {
                item.style.backgroundColor = "#c0c0c0";
                item.style.border = "2px solid black";
            }
        });
    }
}
