import * as functions from "firebase-functions";
import * as admin from "firebase-admin";

admin.initializeApp();

// replaces "pizza" text with 🍕 in {userId:value} value text
export const replacePizzaWithEmoji = functions.database
.ref('/messages/{chatId}/{message}')
.onCreate((snapshot, context) => {
    const userValue = snapshot.val();
    const text = addPizzazz(userValue);
    return snapshot.ref.set(text);
})

function addPizzazz(text: string): string{
    return text.replace(/\bpizza\b/g, `🍕`);
}

export const onFakeMessagesRequest = functions.https.onRequest((request, response) => {
    createFakeMessages();
    response.send("messages started creating");
})

async function createFakeMessages(): Promise<void> {
    await sleep(8000);
    const db = admin.database();
    for(var i = 0; i < 5; i++){
        const ref = db.ref(`/messages/chat1/message${i}`);
        await ref.set("I want some some pizza");
        await sleep(3000);
    }
}

function sleep(time: number): Promise<void> {
    return new Promise(resolve => setTimeout(resolve, time));
}