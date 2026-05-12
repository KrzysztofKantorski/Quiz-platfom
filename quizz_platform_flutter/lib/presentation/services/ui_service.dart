import 'package:flutter/material.dart';
import '../../data/models/question.dart';
import '../../data/models/answer.dart';
import '../../styles/app_colors.dart';

class UIService {
  
  // --- 1. POWIADOMIENIA (SNACKBARS) ---
  // Używane do szybkich komunikatów o sukcesie lub błędzie

  static void showSuccessSnackBar(BuildContext context, String message) {
    _showSnackBar(context, message, AppColors.success);
  }

  static void showErrorSnackBar(BuildContext context, String message) {
    _showSnackBar(context, message, AppColors.error);
  }

  static void _showSnackBar(BuildContext context, String message, Color color) {
    ScaffoldMessenger.of(context).showSnackBar(
      SnackBar(
        content: Text(message, style: const TextStyle(fontWeight: FontWeight.bold)),
        backgroundColor: color,
        behavior: SnackBarBehavior.floating,
        shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(10)),
        duration: const Duration(seconds: 2),
      ),
    );
  }

  // --- 2. DIALOGI OCZEKIWANIA (DLA MAIN / PLAY PAGE) ---

  static void showLoadingDialog(BuildContext context) {
    showDialog(
      context: context,
      barrierDismissible: false,
      builder: (context) => const Center(
        child: CircularProgressIndicator(color: AppColors.accent),
      ),
    );
  }

  // --- 3. DIALOGI DECYZYJNE (DLA MAIN PAGE) ---

  static void showConfirmDeleteDialog({
    required BuildContext context,
    required VoidCallback onConfirm,
  }) {
    showDialog(
      context: context,
      builder: (context) => AlertDialog(
        backgroundColor: AppColors.primary,
        shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(15)),
        title: const Text("Usuń Quiz", style: TextStyle(color: Colors.white)),
        content: const Text(
          "Czy na pewno chcesz trwale usunąć ten quiz? Tej operacji nie da się cofnąć.", 
          style: TextStyle(color: AppColors.textSecondary)
        ),
        actions: [
          TextButton(
            onPressed: () => Navigator.pop(context), 
            child: const Text("Anuluj", style: TextStyle(color: Colors.white54))
          ),
          TextButton(
            onPressed: () {
              Navigator.pop(context);
              onConfirm();
            },
            child: const Text("USUŃ", style: TextStyle(color: AppColors.error, fontWeight: FontWeight.bold)),
          ),
        ],
      ),
    );
  }

  // --- 4. DIALOG WYNIKU (DLA PLAY PAGE) ---

  static void showResultDialog({
    required BuildContext context,
    required int score,
    required int maxScore,
    required VoidCallback onReview,
    required VoidCallback onExit,
  }) {
    showDialog(
      context: context,
      barrierDismissible: false,
      builder: (context) => AlertDialog(
        backgroundColor: AppColors.primary,
        shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(15)),
        title: const Text("Quiz Zakończony!", style: TextStyle(color: AppColors.accent, fontWeight: FontWeight.bold)),
        content: Column(
          mainAxisSize: MainAxisSize.min,
          children: [
            const Icon(Icons.emoji_events, color: AppColors.accent, size: 60),
            const SizedBox(height: 16),
            const Text("Twój wynik:", style: TextStyle(color: AppColors.textSecondary)),
            Text("$score / $maxScore", style: const TextStyle(color: Colors.white, fontSize: 32, fontWeight: FontWeight.bold)),
          ],
        ),
        actions: [
          TextButton(
            onPressed: () {
              Navigator.pop(context);
              onReview();
            },
            child: const Text("PRZEJRZYJ", style: TextStyle(color: AppColors.textSecondary)),
          ),
          ElevatedButton(
            style: ElevatedButton.styleFrom(backgroundColor: AppColors.accent),
            onPressed: onExit,
            child: const Text("MENU", style: TextStyle(color: AppColors.primary, fontWeight: FontWeight.bold)),
          ),
        ],
      ),
    );
  }

  // --- 5. DIALOG EDYCJI/DODAWANIA PYTANIA (DLA BUILD QUIZ PAGE) ---

  static void showQuestionDialog({
    required BuildContext context,
    Question? existingQuestion,
    required Function(Question) onSave,
  }) {
    String questionText = existingQuestion?.text ?? "";
    int questionPoints = existingQuestion?.points ?? 1;
    List<Answer> tempAnswers = existingQuestion != null
        ? existingQuestion.answers.map((a) => Answer(text: a.text, isCorrect: a.isCorrect)).toList()
        : [Answer(text: "", isCorrect: true), Answer(text: "", isCorrect: false)];

    showDialog(
      context: context,
      builder: (context) => StatefulBuilder(
        builder: (context, setDialogState) => AlertDialog(
          backgroundColor: AppColors.primary,
          title: Text(
            existingQuestion == null ? "Nowe Pytanie" : "Edytuj Pytanie", 
            style: const TextStyle(color: AppColors.accent)
          ),
          content: SizedBox(
            width: 500,
            child: SingleChildScrollView(
              child: Column(
                mainAxisSize: MainAxisSize.min,
                children: [
                  TextField(
                    onChanged: (v) => questionText = v,
                    controller: TextEditingController(text: questionText)..selection = TextSelection.collapsed(offset: questionText.length),
                    style: const TextStyle(color: Colors.white),
                    decoration: const InputDecoration(labelText: "Treść pytania", labelStyle: TextStyle(color: AppColors.accent)),
                  ),
                  const SizedBox(height: 16),
                  TextField(
                    keyboardType: TextInputType.number,
                    onChanged: (v) => questionPoints = int.tryParse(v) ?? 1,
                    controller: TextEditingController(text: questionPoints.toString()),
                    style: const TextStyle(color: Colors.white),
                    decoration: const InputDecoration(labelText: "Punkty", labelStyle: TextStyle(color: AppColors.accent)),
                  ),
                  const Padding(
                    padding: EdgeInsets.symmetric(vertical: 15),
                    child: Divider(color: AppColors.accent),
                  ),
                  
                  // Odpowiedzi
                  ...tempAnswers.asMap().entries.map((entry) {
                    int idx = entry.key;
                    return Padding(
                      padding: const EdgeInsets.only(bottom: 8.0),
                      child: Row(
                        children: [
                          Checkbox(
                            value: tempAnswers[idx].isCorrect,
                            onChanged: (v) {
                              setDialogState(() {
                                for (var a in tempAnswers) { a.isCorrect = false; }
                                tempAnswers[idx].isCorrect = v!;
                              });
                            },
                            activeColor: AppColors.accent,
                          ),
                          Expanded(
                            child: TextField(
                              onChanged: (v) => tempAnswers[idx].text = v,
                              controller: TextEditingController(text: tempAnswers[idx].text),
                              style: const TextStyle(color: Colors.white),
                              decoration: InputDecoration(hintText: "Odpowiedź ${idx + 1}", hintStyle: const TextStyle(color: Colors.white30)),
                            ),
                          ),
                          if (tempAnswers.length > 2) // Minimum 2 odpowiedzi
                            IconButton(
                              icon: const Icon(Icons.remove_circle, color: AppColors.error),
                              onPressed: () => setDialogState(() => tempAnswers.removeAt(idx)),
                            )
                        ],
                      ),
                    );
                  }),
                  
                  TextButton.icon(
                    onPressed: () => setDialogState(() => tempAnswers.add(Answer(text: "", isCorrect: false))),
                    icon: const Icon(Icons.add, color: AppColors.accent),
                    label: const Text("Dodaj odpowiedź", style: TextStyle(color: AppColors.accent)),
                  ),
                ],
              ),
            ),
          ),
          actions: [
            TextButton(
              onPressed: () => Navigator.pop(context), 
              child: const Text("Anuluj", style: TextStyle(color: Colors.white54))
            ),
            ElevatedButton(
              style: ElevatedButton.styleFrom(backgroundColor: AppColors.accent),
              onPressed: () {
                if (questionText.trim().isEmpty) {
                  showErrorSnackBar(context, "Pytanie nie może być puste!");
                  return;
                }
                onSave(Question(
                  id: existingQuestion?.id,
                  text: questionText,
                  points: questionPoints,
                  answers: tempAnswers,
                ));
                Navigator.pop(context);
              },
              child: Text(existingQuestion == null ? "Dodaj" : "Zapisz", 
                          style: const TextStyle(color: AppColors.primary, fontWeight: FontWeight.bold)),
            ),
          ],
        ),
      ),
    );
  }
}