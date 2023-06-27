import { WordAnswer } from "../dto/word-answer";

export interface WordResponse {
    wordId: number;
    word: string;
    meaning: string;
    percentage:string;
    writingInLanguage:string;
    wordAnswers: WordAnswer[];
}