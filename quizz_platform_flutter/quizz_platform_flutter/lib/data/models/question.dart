import 'answer.dart';

class Question {
  final int? id;
  final String text;
  final int points;
  final List<Answer> answers;

  Question({
    this.id,
    required this.text,
    required this.points,
    required this.answers,
  });

  factory Question.fromJson(Map<String, dynamic> json) {
    return Question(
      id: json['id'],
      text: json['text'] ?? '',
      points: json['points'] ?? 0,
      answers: json['answers'] != null
          ? (json['answers'] as List)
              .map((a) => Answer.fromJson(a))
              .toList()
          : [],
    );
  }

  Map<String, dynamic> toJson() {
    return {
      if (id != null) 'id': id,
      'text': text,
      'points': points,
      'answers': answers.map((a) => a.toJson()).toList(),
    };
  }
}