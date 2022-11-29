const hubConnection = new signalR.HubConnectionBuilder()
    .withUrl("/ws")
    .build();

function getMessage(message) {
    let messageContainer = document.querySelector('.mes-container');
    let date = new Date(message.dateSend);
    let dateSend = `${date.getMonth() + 1}.${date.getDate()}.${date.getFullYear()} ${date.getHours()}:${date.getMinutes()}`;
    let messageHtml = `
        <div class="row message notification-mes" onclick="openMes(this);removeNoticeMessage(this)" data-bs-toggle="modal" data-bs-target="#fff">
            <div class="row mes-header">Header: ${message.header}</div>
            <div class="row mes-author">Author: ${message.author}</div>
            <div class="row mes-date">DateSend: ${dateSend}</div>
            <div class="row mes-body">${message.body}</div>
        </div>
    `;
    let allMessages = messageContainer.innerHTML;
    messageHtml += allMessages;
    messageContainer.innerHTML = messageHtml;
}

function openMes(elem) {
    let header = elem.getElementsByClassName('mes-header')[0].innerHTML;
    let author = elem.getElementsByClassName('mes-author')[0].innerHTML;
    let date = elem.getElementsByClassName('mes-date')[0].innerHTML;
    let body = elem.getElementsByClassName('mes-body')[0].innerHTML;
    let modalTitle = document.getElementById('modal-header')
    let modalSender = document.getElementById('modal-author')
    let modalDate = document.getElementById('modal-date');
    let modalBody = document.getElementById('modal-body');
    modalTitle.innerHTML = header;
    modalSender.innerHTML = author;
    modalDate.innerHTML = date;
    modalBody.innerHTML = body;
}

function removeNoticeMessage(elem) {
    elem.classList.remove('notification-mes');
}

async function getAllNames() {
    let response = await fetch("Chat/GetNames/", {
        method: 'POST',
        body: JSON.stringify(),
        headers: {
            "Content-Type": "application/json"
        }
    })

    return response.json()
}

async function autocomplete(inp) {
    let arr = await getAllNames()
    let currentFocus;
    inp.addEventListener("input", function (e) {
        let a, b, i, val = this.value;
        closeAllLists();
        if (!val) {
            return false;
        }
        currentFocus = -1;
        a = document.createElement("DIV");
        a.setAttribute("id", this.id + "autocomplete-list");
        a.setAttribute("class", "autocomplete-items");
        this.parentNode.appendChild(a);
        for (i = 0; i < arr.length; i++) {
            if (arr[i].substr(0, val.length).toUpperCase() == val.toUpperCase()) {
                b = document.createElement("DIV");
                b.innerHTML = "<strong>" + arr[i].substr(0, val.length) + "</strong>";
                b.innerHTML += arr[i].substr(val.length);
                b.innerHTML += "<input type='hidden' value='" + arr[i] + "'>";
                b.addEventListener("click", function (e) {
                    inp.value = this.getElementsByTagName("input")[0].value;
                    closeAllLists();
                });
                a.appendChild(b);
            }
        }
    });

    inp.addEventListener("keydown", function (e) {
        var x = document.getElementById(this.id + "autocomplete-list");
        if (x) x = x.getElementsByTagName("div");
        if (e.keyCode == 40) {
            currentFocus++;
            addActive(x);
        } else if (e.keyCode == 38) {
            currentFocus--;
            addActive(x);
        } else if (e.keyCode == 13) {
            e.preventDefault();
            if (currentFocus > -1) {
                if (x) x[currentFocus].click();
            }
        }
    });

    function addActive(x) {
        if (!x) return false;
        removeActive(x);
        if (currentFocus >= x.length) currentFocus = 0;
        if (currentFocus < 0) currentFocus = (x.length - 1);
        x[currentFocus].classList.add("autocomplete-active");
    }

    function removeActive(x) {
        for (let i = 0; i < x.length; i++) {
            x[i].classList.remove("autocomplete-active");
        }
    }

    function closeAllLists(elmnt) {
        let x = document.getElementsByClassName("autocomplete-items");
        for (let i = 0; i < x.length; i++) {
            if (elmnt != x[i] && elmnt != inp) {
                x[i].parentNode.removeChild(x[i]);
            }
        }
    }

    document.addEventListener("click", function (e) {
        closeAllLists(e.target);
    });
}

let recipientInp = document.getElementById('input-recipient');
autocomplete(recipientInp);

let sendBtn = document.getElementById('btn-send');
sendBtn.addEventListener('click', async () => {
    let recipient = document.getElementById('input-recipient');
    let header = document.getElementById('input-header');
    let body = document.getElementById('textarea-body');
    let error = document.getElementById('err-span');
    if (recipient.value !== '' && header.value !== '' && body.value !== '') {
        error.innerHTML = '';
        error.classList.remove('error-item');
        hubConnection.invoke('Send', body.value, recipient.value, header.value);
        recipient.value = '';
        header.value = '';
        body.value = '';
    } else {
        error.textContent = 'Fill the fields';
        error.classList.add('error-item');
    }
    
})

window.onload = async () => {
    await hubConnection.start();
}

hubConnection.on('Receive', function (message) {
    getMessage(message);
})