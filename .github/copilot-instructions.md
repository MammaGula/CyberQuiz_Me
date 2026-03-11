# Copilot Instructions

## Project Guidelines
- User can only work on their assigned areas: DAL and API layers (not UI).
- User prefers responses in Thai, regardless of the language used for communication.
- User and the group do not want to reduce comments in the code, as the instructor appreciates comments; therefore, a high number of comments should not be considered a problem during the review process.

## Method Review Guidelines
- สำหรับการรีวิวและลบเมธอดใน repositories ให้เก็บเฉพาะเมธอดที่ QuizService เรียกอยู่ตอนนี้หรือจะใช้แทน GetAllAsync() ทันที และให้ลบเมธอด WithDetails ที่ไม่มี caller กับเมธอดซ้ำแนวคิดที่ยังไม่มี caller.
- ถ้าเมธอดใน DAL ยังไม่มี caller ตอนนี้แต่ทีมกำลังจะ refactor BLL ให้เรียกใช้ในเร็ว ๆ นี้ ให้เก็บเมธอดนั้นไว้และอย่าลบทิ้งก่อน.