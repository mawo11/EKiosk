class MessageClient {
    private _handlers: any
    private _oneTimeHandlers: any

    public constructor() {
        this._handlers = {}
        this._oneTimeHandlers = {}

        window.chrome?.webview?.addEventListener('message', (e: any) => {
            console.log('message', e.data)
            const message = JSON.parse(e.data)
            if (message.event) {
                const eventId = '$' + message.event

                if (this._oneTimeHandlers[eventId]) {
                    this._oneTimeHandlers[eventId](message.payload)
                    delete this._handlers[eventId]
                    return
                }

                if (this._handlers[eventId]) {
                    this._handlers[eventId](message.payload)
                }
            }
        })
    }

    public send(event: string, payload?: any) {
        const message = {
            event,
            payload: payload ? JSON.stringify(payload) : null
        };

        console.log('sending message', message)

        window.chrome?.webview?.postMessage(JSON.stringify(message));
    }

    public on(eventId: string, fn: any) {
        const eventIdKey = '$' + eventId

        if (typeof this._handlers[eventIdKey] === 'function') {
            return
        }

        this._handlers[eventIdKey] = fn
    }

    public once(eventId: string, fn: any) {
        const eventIdKey = '$' + eventId

        if (typeof this._oneTimeHandlers[eventIdKey] === 'function') {
            return
        }

        this._oneTimeHandlers[eventIdKey] = fn
    }

    public off(eventId: string) {
        try {
            delete this._handlers['$' + eventId]
        } catch { }
    }
}

export const messageClient = new MessageClient();
