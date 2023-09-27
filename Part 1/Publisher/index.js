import net from "net";
import readline from "readline";

const brokerHost = '127.0.0.1'; // Adresa IP a brokerului
const brokerPort = 9000; // Portul la care se conectează clientul

const client = new net.Socket(); // Creează un socket client

const rl = readline.createInterface({
    input: process.stdin, // Inputul pentru citirea de la tastatură
    output: process.stdout // Outputul pentru afișarea la consolă
});

// Se conectează la broker
client.connect(brokerPort, brokerHost, () => {
    console.log('Connected to Broker');
    getUserInput(); // Apelează funcția pentru a obține inputul de la utilizator
});

// Funcție pentru a obține inputul utilizatorului pentru topic și mesaj
function getUserInput() {
    rl.question('Enter the topic (or type "exit" to quit): ', (topic) => {
        if (topic.toLowerCase() === 'exit') {
            client.end(); // Închide socketul client
            rl.close(); // Închide interfața de citire
        } else {
            rl.question('Enter the message: ', (message) => {
                const xmlData = `<Content><Topic>${topic}</Topic><Message>${message}</Message></Content>`;

                // Trimite datele XML către broker
                client.write(xmlData);

                // Obține următorul input
                getUserInput();
            });
        }
    });
}

// Gestionează datele primite de la broker
client.on('data', (data) => {
    console.log('Received response from Broker:', data.toString());
});

// Gestionează deconectarea
client.on('close', () => {
    console.log('Connection to Broker closed');
});
