var inbox = [];
var connection = new signalR.HubConnectionBuilder().withUrl("/MessageHub").build();

const grid = new gridjs.Grid({
    columns: [
        {
            name: 'Messageindex',
            hidden: true
        },
        'From',
        'Title',
        'Date and time',
    ],
    style: {
        table: {
            width: '100%'
        },
        td: {
            'overflow-wrap': 'normal',
            cursor: 'pointer'
        }
    },
    className: {
        table: 'table table-hover',
        thead: 'table-dark',
    },
    server: {
        url: '/Home/GetMessages/' + username,
        method: 'GET',
        then: messages => {
            inbox = messages;
            let data = messages.map((message, index) => [index, message.sender, message.title, message.createdAt]);
            return data.reverse();
        }
    }
}).render($('#table-wrapper')[0]);

grid.on('rowClick', (e, row) => {
    let index = row.cells[0].data;
    let message = inbox[index];
    $('#message-title').text(message.title);
    $('#message-sender').text(message.sender);
    $('#message-created-at').text(message.createdAt);
    $('#message-body').text(message.body);
    $('#message-modal').modal('show');
});

$('#recipent-input').autocomplete({
    source: '/Home/SearchUsers'
});

connection.on("ReceiveMessage", message => {
    inbox.push(message);
    let newData = inbox.map((message, index) => [index, message.sender, message.title, message.createdAt]);
    newData.reverse();
    grid.updateConfig({
        data: newData
    }).forceRender();
});

connection.start().then(() => {
    connection.invoke("Connect", username).catch(error => console.error(error));
});