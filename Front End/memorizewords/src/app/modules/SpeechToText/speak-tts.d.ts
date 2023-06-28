// declare module 'speak-tts' {
//   export interface SpeakOptions {
//     text: string;
//     queue?: boolean;
//     listeners?: {
//       onstart?: () => void;
//       onend?: () => void;
//       onresume?: () => void;
//       onboundary?: (event: SpeechSynthesisEvent) => void;
//       onerror?: (error: SpeechSynthesisErrorEvent) => void;
//     };
//   }

//   export default class SpeakTTS {
//     constructor();

//     init(): Promise<boolean>;
//     speak(options: SpeakOptions): Promise<void>;
//     pause(): void;
//     resume(): void;
//     cancel(): void;
//     getVoices(): SpeechSynthesisVoice[];
//     setVoice(voiceURI: string): void;
//   }
// }
