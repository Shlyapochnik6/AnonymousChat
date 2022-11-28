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