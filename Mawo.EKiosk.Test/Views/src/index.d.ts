import { AppDb, electron } from './api';

declare global {
    interface Window {
        chrome?: {
            webview?: {
                addEventListener: (
                    message: string,
                    handler: (event: any) => void, // not sure yet what the event type should be
                ) => void;
                postMessage: (message: string) => void;
            };
        };
    }
}