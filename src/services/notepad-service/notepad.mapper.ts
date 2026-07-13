import { INotepad } from './types/notepad.type';

export class NotepadMapper {

    public static map(notepad: any): INotepad {
        return {
            identifier: notepad.Identifier,
            note: notepad.Note,
            createdAt: new Date(notepad.CreatedAt),
            updatedAt: new Date(notepad.UpdatedAt)
        };
    }
}