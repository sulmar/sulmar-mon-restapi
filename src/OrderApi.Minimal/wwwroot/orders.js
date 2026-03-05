const orderId = "c77cf2e3-1076-4f5d-b6e3-bb4144b5c465"

const connection = new signalR.HubConnectionBuilder()
    .withUrl("/signalr/orders")
    .build();

connection.on("OrderStatusChanged", (data) => {
    console.log("Order updated: ", data);
    document.getElementById("status").innerHTML = data.status;
});

async function start() {
    try {
        console.log("connecting...");
        await connection.start();
        console.log("Connected.");

        connection.invoke("Subscribe", orderId);
        console.log("Subscribed to order", orderId);
    }
    catch (err) {
        console.log(err);
    }
}

start();
