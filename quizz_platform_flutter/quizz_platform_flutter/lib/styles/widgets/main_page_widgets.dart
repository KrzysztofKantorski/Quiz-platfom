import 'package:flutter/material.dart';
import 'package:quizz_platform_flutter/presentation/pages/build_quiz_page.dart';
import '../../styles/app_colors.dart';
import '../../data/models/quiz.dart';


class MainHeader extends StatelessWidget {
  final int count;
  final VoidCallback onRefresh;

  const MainHeader({super.key, required this.count, required this.onRefresh});

  @override
  Widget build(BuildContext context) {
    return Row(
      mainAxisAlignment: MainAxisAlignment.spaceBetween,
      children: [
        Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            const Text("Twoje Quizy", 
                style: TextStyle(color: AppColors.accent, fontSize: 32, fontWeight: FontWeight.bold)),
            Text("Łącznie: $count", style: const TextStyle(color: Colors.white60, fontSize: 16)),
          ],
        ),
        Row(
          children: [
            IconButton(
              icon: const Icon(Icons.refresh, color: AppColors.accent, size: 28),
              onPressed: onRefresh,
            ),
            const SizedBox(width: 10),
            ElevatedButton.icon(
              style: ElevatedButton.styleFrom(
                backgroundColor: Colors.greenAccent,
                foregroundColor: AppColors.primary,
                padding: const EdgeInsets.symmetric(horizontal: 20, vertical: 12),
                shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(15)),
              ),
              onPressed: () => Navigator.push(context, MaterialPageRoute(builder: (context) => const BuildQuizPage())),
              icon: const Icon(Icons.add),
              label: const Text("NOWY QUIZ", style: TextStyle(fontWeight: FontWeight.bold)),
            ),
          ],
        ),
      ],
    );
  }
}

class EmptyQuizPlaceholder extends StatelessWidget {
  const EmptyQuizPlaceholder({super.key});

  @override
  Widget build(BuildContext context) {
    return Center(
      child: Column(
        mainAxisAlignment: MainAxisAlignment.center,
        children: [
          Icon(Icons.quiz_outlined, size: 80, color: Colors.white.withValues(alpha: 0.2)),
          const SizedBox(height: 20),
          const Text("Brak quizów. Stwórz swój pierwszy!", 
              style: TextStyle(color: Colors.white54, fontSize: 18)),
        ],
      ),
    );
  }
}

class MainActionButton extends StatelessWidget {
  final String label;
  final bool enabled;
  final VoidCallback onPressed;

  const MainActionButton({super.key, required this.label, required this.enabled, required this.onPressed});

  @override
  Widget build(BuildContext context) {
    return Padding(
      padding: const EdgeInsets.symmetric(vertical: 8.0, horizontal: 40),
      child: SizedBox(
        width: double.infinity,
        height: 45,
        child: ElevatedButton(
          style: ElevatedButton.styleFrom(
            backgroundColor: enabled ? AppColors.accent.withValues(alpha: 0.1) : Colors.grey.withValues(alpha: 0.1),
            side: BorderSide(color: enabled ? AppColors.accent : Colors.grey, width: 2),
            shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(10)),
          ),
          onPressed: enabled ? onPressed : null,
          child: Text(label, style: TextStyle(color: enabled ? AppColors.accent : Colors.grey, fontWeight: FontWeight.bold)),
        ),
      ),
    );
  }
}

class QuizListCard extends StatelessWidget {
  final Quiz quiz;
  final bool isSelected;
  final VoidCallback onTap;
  final VoidCallback onPlay;
  final VoidCallback onEdit;
  final VoidCallback onDelete;

  const QuizListCard({
    super.key, 
    required this.quiz, 
    required this.isSelected, 
    required this.onTap, 
    required this.onPlay, 
    required this.onEdit, 
    required this.onDelete
  });

  @override
  Widget build(BuildContext context) {
    return Card(
      color: isSelected ? AppColors.accent.withValues(alpha:0.2) : Colors.white.withValues(alpha:0.05),
      margin: const EdgeInsets.symmetric(vertical: 6),
      child: ListTile(
        onTap: onTap,
        title: Text(quiz.title, style: const TextStyle(color: Colors.white, fontWeight: FontWeight.bold)),
        subtitle: Text(quiz.description, maxLines: 1, overflow: TextOverflow.ellipsis, style: const TextStyle(color: Colors.white60)),
        trailing: Row(
          mainAxisSize: MainAxisSize.min,
          children: [
            IconButton(icon: const Icon(Icons.play_arrow_rounded, color: Colors.greenAccent, size: 30), onPressed: onPlay),
            IconButton(icon: const Icon(Icons.edit, color: AppColors.accent), onPressed: onEdit),
            IconButton(icon: const Icon(Icons.delete, color: AppColors.error), onPressed: onDelete),
          ],
        ),
      ),
    );
  }
}