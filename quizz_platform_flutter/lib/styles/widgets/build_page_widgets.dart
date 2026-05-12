import 'package:flutter/material.dart';
import '../../styles/app_colors.dart';

class BuildTextField extends StatelessWidget {
  final String hint;
  final TextEditingController controller;
  final int maxLines;

  const BuildTextField({super.key, required this.hint, required this.controller, this.maxLines = 1});

  @override
  Widget build(BuildContext context) {
    return TextField(
      controller: controller,
      maxLines: maxLines,
      style: const TextStyle(color: Colors.white),
      decoration: InputDecoration(
        hintText: hint,
        hintStyle: const TextStyle(color: Colors.white54),
        filled: true,
        fillColor: AppColors.primary,
        border: OutlineInputBorder(borderRadius: BorderRadius.circular(10), borderSide: BorderSide.none),
      ),
    );
  }
}

class QuestionListTile extends StatelessWidget {
  final String text;
  final int points;
  final VoidCallback onEdit;
  final VoidCallback onDelete;

  const QuestionListTile({super.key, required this.text, required this.points, required this.onEdit, required this.onDelete});

  @override
  Widget build(BuildContext context) {
    return Card(
      color: AppColors.primary.withValues(alpha: 0.5),
      child: ListTile(
        title: Text(text, style: const TextStyle(color: Colors.white)),
        subtitle: Text("$points pkt", style: const TextStyle(color: Colors.white60)),
        trailing: Row(
          mainAxisSize: MainAxisSize.min,
          children: [
            IconButton(icon: const Icon(Icons.edit, color: AppColors.accent), onPressed: onEdit),
            IconButton(icon: const Icon(Icons.delete, color: AppColors.error), onPressed: onDelete),
          ],
        ),
      ),
    );
  }
}